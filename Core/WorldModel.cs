// ***********************************************************************
// Assembly         : Core
// Author           : pbosch
// Created          : 03-18-2019
//
// Last Modified By : pbosch
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="WorldModel.cs" company="Highpoint Software Systems, LLC">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
#define ACCOMODATE_AIR_TRAVEL
#define USE_PARALLEL

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Highpoint.Sage.SimCore;
using Highpoint.Sage.SystemDynamics;

namespace Core
{
    public struct Coordinates
    {
        public int X;
        public int Y;

        public override bool Equals(object obj)
        {
            if (obj is Coordinates)
            {
                return ((Coordinates)obj).X == X && ((Coordinates)obj).Y == Y;
            }
            return false;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        internal double DistanceTo(Coordinates destination)
        {
            return Math.Sqrt(Math.Pow(X-destination.X, 2) + Math.Pow(Y - destination.Y, 2));
        }
    }

    /// <summary>
    /// Class WorldModel.
    /// </summary>
    public class WorldModel
    {
        private static readonly double EPSILON = 0.00001;
        private static Random m_random = new Random();
        public GlobalAirTravelPolicy GlobalAirTravelPolicy = new GlobalAirTravelPolicy();

        private readonly Dictionary<string, SimCountryData> m_countryDataByCountryCodes; 
        /// <summary>
        /// The m data
        /// </summary>
        private readonly MapData m_mapData;
        /// <summary>
        /// The m nodes
        /// </summary>
        private DiseaseNode[,] m_nodes;
        /// <summary>
        /// The m size
        /// </summary>
        private readonly Size m_size;

        /// <summary>
        /// The m controller
        /// </summary>
        private readonly Controller m_controller;

        /// <summary>
        /// Gets the execution parameters.
        /// </summary>
        /// <value>The execution parameters.</value>
        public ExecParameters ExecutionParameters { get; private set; }
        /// <summary>
        /// Gets the executive.
        /// </summary>
        /// <value>The executive.</value>
        public IExecutive Executive { get; private set; }
        /// <summary>
        /// Gets the busy routes.
        /// </summary>
        /// <value>The busy routes.</value>
        public List<RouteData> BusyRoutes => m_mapData.BusyRoutes;
        /// <summary>
        /// Gets or sets a value indicating whether [no air travel from to known infected].
        /// </summary>
        /// <value><c>true</c> if [no air travel from to known infected]; otherwise, <c>false</c>.</value>
        public bool NoAirTravelFromToKnownInfected { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [no air travel at all].
        /// </summary>
        /// <value><c>true</c> if [no air travel at all]; otherwise, <c>false</c>.</value>
        public bool NoAirTravelAtAll { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldModel"/> class.
        /// </summary>
        /// <param name="mapData">The map data.</param>
        /// <param name="countryData">The country data.</param>
        /// <param name="ortCounts">For each country, the number of Outbreak Response Teams it has, and the color they are rendered in.</param>
        /// <param name="execParameters">The execute parameters.</param>
        public WorldModel(MapData mapData, List<SimCountryData> countryData, Dictionary<string, Tuple<int,Color>> ortCounts = null, ExecParameters execParameters = null)
        {
            
            Executive = ExecFactory.Instance.CreateExecutive(ExecType.FullFeatured);
            ExecutionParameters = execParameters?? new ExecParameters();
            m_controller = new Controller(this);

            m_countryDataByCountryCodes = new Dictionary<string, SimCountryData>();
            countryData.ForEach(n=> m_countryDataByCountryCodes.Add(n.CountryCode, n));

            m_ortCounts = ortCounts??new Dictionary<string, Tuple<int, Color>>();
            m_mapData = mapData;
            m_nodes = new DiseaseNode[m_mapData.Width, m_mapData.Height];
            m_size = new Size(m_mapData.Width, m_mapData.Height);

            for (int x = 0; x < m_mapData.Width; x++)
            {
                for (int y = 0; y < m_mapData.Height; y++)
                {
                    double density = m_mapData.CellData[x, y].PopulationDensity;
                    if (Math.Abs(density - CellData.NO_DATA) < 0.01) density = 0.0;
                    double landArea = m_mapData.CellData[x, y].LandArea;
                    double waterArea = m_mapData.CellData[x, y].WaterArea;
                    Locale locale = new Locale(Locale.DEFAULT);
                    switch (m_mapData.CellData[x, y].CellState)
                    {
                        case CellState.Unknown:
                            locale.Population = 0;
                            locale.Area = Locale.DEFAULT.Area;
                            break;
                        case CellState.Occupied:
                            locale.Area = landArea + (Math.Abs(waterArea - CellData.NO_DATA) < 0.01 ? 0 : waterArea);
                            if (locale.Area <= 0.001) locale.Area = Locale.DEFAULT.Area;
                            locale.Population = density * landArea;
                            break;
                        case CellState.Unoccupied:
                            locale.Population = 0;
                            break;
                        case CellState.Ocean:
                            locale.Population = 0;
                            break;
                        default:
                            break;
                    }

                    double lat, lon;
                    m_mapData.DataXYToLatLon(x,y,out lat, out lon);
                    m_nodes[x, y] = new DiseaseNode(Policy.DEFAULT, locale, Disease.DEFAULT);

                    // Bidirectional Cross-Reference.
                    m_nodes[x, y].MapCell = m_mapData.CellData[x, y];
                    m_mapData.CellData[x, y].DiseaseModel = m_nodes[x, y];
                }
            }

            CreateAndAssignGovernments();
        }

        private readonly List<NationalGovernment> m_governments = new List<NationalGovernment>();
        private readonly Dictionary<string, Tuple<int, Color>> m_ortCounts;

        public List<NationalGovernment> Governments => m_governments;

        private List<OutbreakResponseTeam> m_allORTs; 

        private void CreateAndAssignGovernments()
        {
            m_allORTs = new List<OutbreakResponseTeam>();
            ILineWriter console = Locator.Resolve<ILineWriter>("console");
            Dictionary<string, List<Coordinates>> nodeToCountryMapping = new Dictionary<string, List<Coordinates>>();

            for (int x = 0; x < m_mapData.Width; x++)
            {
                for (int y = 0; y < m_mapData.Height; y++)
                {
                    DiseaseNode dn = m_nodes[x, y];
                    string cc = dn.MapCell.CountryCode;
                    if (cc != null && !"--".Equals(cc))
                    {
                        List<Coordinates> diseaseNodeCoordinates;
                        if (!nodeToCountryMapping.TryGetValue(cc, out diseaseNodeCoordinates))
                        {
                            diseaseNodeCoordinates = new List<Coordinates>();
                            nodeToCountryMapping.Add(cc, diseaseNodeCoordinates);
                        }
                        diseaseNodeCoordinates.Add(new Coordinates {X = x, Y = y});
                    }
                }
            }

            Dictionary<string, NationalGovernment> govts = new Dictionary<string, NationalGovernment>();
            for (int x = 0; x < m_mapData.Width; x++)
            {
                for (int y = 0; y < m_mapData.Height; y++)
                {
                    DiseaseNode dn = m_nodes[x, y];
                    string countryCode = dn.MapCell.CountryCode;
                    if ("--".Equals(countryCode)) continue;
                    if (countryCode == null) continue;
                    if (!m_countryDataByCountryCodes.ContainsKey(countryCode))
                    {
                        //console.WriteLine($"No country data for country code \"{countryCode}\".");
                        continue;
                    }
                    SimCountryData simCountryData = m_countryDataByCountryCodes[countryCode];
                    if (nodeToCountryMapping.ContainsKey(dn.MapCell.CountryCode))
                    {
                        if (!govts.ContainsKey(countryCode))
                        {
                            NationalGovernment ng = new NationalGovernment(this)
                            {
                                WorldModel = this,
                                SimCountryData = m_countryDataByCountryCodes[countryCode],
                                DiseaseNodeCoordinates = nodeToCountryMapping[countryCode]
                            };

                            if (m_ortCounts.ContainsKey(simCountryData.CountryCode))
                            {
                                var ortCount = m_ortCounts[simCountryData.CountryCode];
                                int numberOfResponseTeams = ortCount.Item1;
                                for (int i = 0; i < numberOfResponseTeams; i++)
                                {
                                    int randomDiseaseNode = m_random.Next(0, ng.DiseaseNodeCoordinates.Count - 1);
                                    Coordinates initialCoordinates = ng.DiseaseNodeCoordinates[randomDiseaseNode];
                                    OutbreakResponseTeam ort = 
                                        new OutbreakResponseTeam(simCountryData.CountryCode, i, ortCount.Item2, this, initialCoordinates);
                                    ng.ResponseTeams.Add(ort);
                                    simCountryData.ResponseTeams.Add(ort);
                                    m_allORTs.Add(ort);
                                }
                            }

                            simCountryData.Government = ng;
                            govts.Add(countryCode, ng);
                            ng.UpdateDiseaseNodes(true);
                        }
                    }
                    else
                    {
                        console.WriteLine($"Could not find a list of nodes for country code {simCountryData.CountryCode}");
                    }

                }
            }
            m_governments.AddRange(govts.Values);
        }

        public DiseaseNode[,] DiseaseNodes => m_nodes;
        /// <summary>
        /// Gets the map data.
        /// </summary>
        /// <value>The map data.</value>
        public MapData MapData => m_mapData;

        /// <summary>
        /// Announces when a new iteration is available.
        /// </summary>
        public event Action<DiseaseNode[,], double[], List<RouteData>, List<OutbreakResponseTeam>> NewIterationAvailable;

        /// <summary>
        /// Runs the specified manual reset event1.
        /// </summary>
        /// <param name="manualResetEvent1">The manual reset event1.</param>
        /// <param name="manualResetEvent2">The manual reset event2.</param>
        public void Run(ManualResetEvent manualResetEvent1, ManualResetEvent manualResetEvent2)
        {
            while (m_nodes[0, 0].Start + (m_nodes[0, 0].TimeSliceNdx * m_nodes[0, 0].TimeStep) < m_nodes[0, 0].Finish)
            {
                Update(Executive, manualResetEvent1, manualResetEvent2);
            }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public Size Size => m_size;

        /// <summary>
        /// Nodes at.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>DiseaseNode.</returns>
        public DiseaseNode NodeAt(int x, int y)
        {
            return m_nodes[x, y];
        }

        /// <summary>
        /// Infects at.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="howMuch">The how much.</param>
        public void InfectAt(int x, int y, double howMuch = 1.0)
        {
            m_nodes[x, y].ContagiousAsymptomatic += howMuch;
        }


        /// <summary>
        /// Updates the specified execute.
        /// </summary>
        /// <param name="exec">The execute.</param>
        /// <param name="userdata">The userdata.</param>
        internal void Update(IExecutive exec, object userdata)
        {
            Update(exec);
        }

        /// <summary>
        /// Updates the world model by one SD timeslice.
        /// </summary>
        /// <param name="exec">The executive under which this update is being done.</param>
        /// <param name="manualResetEvent1">The manual reset event1.</param>
        /// <param name="manualResetEvent2">The manual reset event2.</param>
        internal void Update(IExecutive exec, ManualResetEvent manualResetEvent1 = null, ManualResetEvent manualResetEvent2 = null)
        {
            ILineWriter console = Locator.Resolve<ILineWriter>("console");
            m_governments.ForEach(n=>n.UpdateDiseaseNodes());
            bool travelOnlyFromToApparentlyDiseaseFree = NoAirTravelFromToKnownInfected;
            manualResetEvent1?.WaitOne();
            manualResetEvent2?.WaitOne();
            //DateTime now = DateTime.Now;
            DiseaseNode[,] newNodes = new DiseaseNode[m_mapData.Width, m_mapData.Height];
            // Progress the simulation one time step.

            int count1 = 0, count2 = 0;

#if AUTO_INOCULATE
            if (m_nodes[0, 0].TimeSliceNdx == 10 && m_districtsOfInterest != null)
            {
                {
                    foreach (var tuple in m_districtsOfInterest)
                    {
                        console.WriteLine($"Inoculating at {tuple.Item1},{tuple.Item2} with patient zero.");
                        m_nodes[tuple.Item1, tuple.Item2].ContagiousAsymptomatic++;
                        console.WriteLine(m_nodes[tuple.Item1, tuple.Item2]);
                    }
                }
            }
#endif

#if USE_PARALLEL
            Parallel.For(0, m_mapData.Width, x =>
            {
#else
            for (int x = 0; x < m_data.Width; x++)
            {
#endif
                for (int y = 0; y < m_mapData.Height; y++)
                {
                    if (!m_mapData.CellData[x, y].CellState.Equals(CellState.Occupied))
                    {
                        if (m_mapData.CellData[x, y].DiseaseModel.Population > 0) Debugger.Break();
                        newNodes[x, y] = m_nodes[x, y];
                        newNodes[x, y].TimeSliceNdx++;
                        Interlocked.Increment(ref count1);
                    }
                    else
                    {
                        //EpidemicNode.m_debug = (x == 243 && y == 240);
                        //if ( x== 243 && y == 240 && m_nodes[x, y].TimeSliceNdx == 63) Debugger.Break();
                        DiseaseNode oldNode = m_nodes[x, y];
                        DiseaseNode newNode = Behavior<DiseaseNode>.RunOneTimeslice(m_nodes[x, y]);
                        newNodes[x, y] = newNode;
                        if (oldNode.ContagiousAsymptomatic < EPSILON && newNode.ContagiousAsymptomatic > EPSILON)
                        {
                            ReportNewlyInfectedNode(new Coordinates() {X=x, Y=y});
                        }
                        Interlocked.Increment(ref count2);
                    }
                }

#if USE_PARALLEL
            });
#else
            } // Non-parallel.
#endif
            //console.WriteLine($"New state computation takes {(DateTime.Now - now).TotalMilliseconds} mSec.");
            //now = DateTime.Now;

            // Now propagate contagiousness to neighbors.
            Random r = new Random();
            int radius = 2;
            double maxDist = Math.Sqrt(2 * radius * radius);
            //int count = 0;
#if _USE_PARALLEL
            Parallel.For(0, m_data.Width, x =>
            {
#else
            for (int x = 0; x < m_mapData.Width; x++)
            {
#endif
                for (int y = 0; y < m_mapData.Height; y++)
                {
                    //count = 0;
                    if (!m_mapData.CellData[x, y].CellState.Equals(CellState.Occupied)) continue;
                    DiseaseNode cursor = m_nodes[x, y];
                    if (!cursor.Quarantined && (cursor.ContagiousAsymptomatic > 0 || cursor.ContagiousSymptomatic > 0))
                    {
                        for (int dx = -radius; dx < radius + 1; dx++)
                        {
                            //count++;
                            // Handle longitudinal wraparound.
                            int tmpDx = dx;
                            if (x + dx < 0) tmpDx += m_mapData.Width;
                            if (x + dx >= m_mapData.Width) tmpDx -= m_mapData.Width;
                            for (int dy = -radius; dy < radius + 1; dy++)
                            {
                                if (y + dy < 0 || y + dy >= m_mapData.Height) continue;
                                if (dx == 0 && dy == 0) continue;
                                double dist = Math.Sqrt(dx * dx + dy * dy);
                                DiseaseNode neighbor = newNodes[x + tmpDx, y + dy];
                                lock (neighbor) // Neighbor could otherwise be under modification by this code from two different cursors.
                                {
                                    if (neighbor.Quarantined || neighbor.Population < 1) continue;
                                    double factor = (1.0 - ((dist*dist)/((maxDist + 1)*(maxDist + 1))));
                                    double traveling = r.NextDouble()*factor*cursor.LocaleData.Mobility*
                                                       cursor.ContagiousAsymptomatic;
                                    double newInfections = Math.Min(traveling*neighbor.ContractionRate,neighbor.Susceptible);
                                    //if (newInfections > 0.5)
                                    {
                                        neighbor.ContagiousAsymptomatic += newInfections;
                                        neighbor.Susceptible -= newInfections;
                                    }

                                    traveling = r.NextDouble()*factor*cursor.LocaleData.Mobility*
                                                cursor.ContagiousSymptomatic*
                                                (1.0 - neighbor.LocaleData.HealthCareEffectiveness);
                                    newInfections = Math.Min(traveling*neighbor.ContractionRate, neighbor.Susceptible);
                                    //if (newInfections > 0.5)
                                    {
                                        neighbor.ContagiousAsymptomatic += newInfections;
                                        neighbor.Susceptible -= newInfections;
                                    }
                                }
                            }
                        }
                    }
                }
#if _USE_PARALLEL
            });
#else
            } // Non-parallel.
#endif

            //console.WriteLine($"Neighbor propagation takes {(DateTime.Now - now).TotalMilliseconds} mSec.");
            //now = DateTime.Now;

            double[] routeTransmittals = new double[0];
#if ACCOMODATE_AIR_TRAVEL
            // Now process Air Travel.
            int canceledFlights = 0;

            if (!NoAirTravelAtAll)
            {
                int whichRoute = 0;
                routeTransmittals = new double[BusyRoutes.Count];
                double nPassengers;
#if _USE_PARALLEL
                Parallel.ForEach(m_data.BusyRoutes, rd =>
#else
                foreach (RouteData rd in m_mapData.BusyRoutes)
#endif
                {
                    DiseaseNode fromWas = m_nodes[rd.From.MapX, rd.From.MapY];
                    DiseaseNode toWas = m_nodes[rd.To.MapX, rd.To.MapY];
                    DiseaseNode fromWillBe = newNodes[rd.From.MapX, rd.From.MapY];
                    DiseaseNode toWillBe = newNodes[rd.To.MapX, rd.To.MapY];

                    nPassengers = double.NegativeInfinity;
                    if (!fromWas.Quarantined && !toWas.Quarantined)
                    {
                        if (!travelOnlyFromToApparentlyDiseaseFree ||
                            (fromWas.ContagiousSymptomatic < 0.2 &&
                             fromWas.NonContagiousInfected < 0.2 &&
                             toWas.NonContagiousInfected < 0.2 &&
                             toWas.NonContagiousInfected < 0.2))
                        {

                            double fromDensity = fromWas.ContagiousAsymptomatic/fromWas.Population;
                            double toDensity = toWas.ContagiousAsymptomatic/toWas.Population;
                            if (fromWas.Population < .01) fromDensity = 0;
                            if (toWas.Population < .01) toDensity = 0;

                            nPassengers = rd.MaxPassengers*Math.Abs(fromDensity - toDensity);
                            if (fromDensity > toDensity)
                            {
                                toWillBe.ContagiousAsymptomatic += nPassengers;
                                fromWillBe.ContagiousAsymptomatic -= Math.Min(fromWillBe.ContagiousAsymptomatic,
                                    nPassengers);
                            }
                            else if (toDensity > fromDensity)
                            {
                                fromWillBe.ContagiousAsymptomatic += nPassengers;
                                toWillBe.ContagiousAsymptomatic -= Math.Min(toWillBe.ContagiousAsymptomatic, nPassengers);
                            }

                            // In some data cases, airports' coordinates place them in Ocean cells. This is a skew/registration issue
                            // with my XY<->LatLong translation, or (e.g. Gran Canaria) because the airport's on a small island. We 
                            // hack this by simply changing it to "Occupied..."
                            // TODO: Fix this: Fix registration or remove the route. (Removing the route is not acceptable 
                            // for registration issues because, e.g. SFO might fall into the ocean. (LOL.)
                            if ( toWillBe.MapCell.CellState != CellState.Occupied ) toWillBe.MapCell.CellState = CellState.Occupied;
                        }
                        else
                        {
                            canceledFlights++;
                        }
                    }
                    lock (routeTransmittals) routeTransmittals[whichRoute++] = nPassengers;

#if _USE_PARALLEL
                });
#else
                } // Non-parallel.
#endif
#endif
            }
            else
            {
                canceledFlights = m_mapData.BusyRoutes.Count;
            }
            m_nodes = newNodes;
            if (canceledFlights > 0)
            {
                if (canceledFlights == m_mapData.BusyRoutes.Count)
                {
                    console.WriteLine($"{Executive.Now.ToLongDateString()} : All air routes canceled.");
                }
                else
                {
                    console.WriteLine(
                        $"{Executive.Now.ToLongDateString()} : {canceledFlights} (of {m_mapData.BusyRoutes.Count}) air routes canceled.");
                }
            }

            m_governments.ForEach(n=>n.Reassess(exec));

            NewIterationAvailable?.Invoke(newNodes, routeTransmittals, BusyRoutes, m_allORTs);
        }

        /// <summary>
        /// Gets the controller.
        /// </summary>
        /// <value>The controller.</value>
        public Controller Controller
        {
            get { return m_controller; }
            //private set { m_controller = value; }
        }

        public SimCountryData CountryForCode(string countryCode)
        {
            SimCountryData cd;
            // TODO: Make this a dictionary.
            m_countryDataByCountryCodes.TryGetValue(countryCode, out cd);
            return cd;
        }

        public void ReportNewlyInfectedNode(Coordinates where)
        {
            OnNewlyInfectedNode?.Invoke(where);
        }

        public event Action<Coordinates> OnNewlyInfectedNode;
    }


    /// <summary>
    /// Class Controller.
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// The m world model
        /// </summary>
        private readonly WorldModel m_worldModel;

        /// <summary>
        /// Enum State
        /// </summary>
        public enum State
        {
            /// <summary>
            /// The ready
            /// </summary>
            Ready, Running, Paused, Stopped

        }

        /// <summary>
        /// The model state
        /// </summary>
        public State ModelState = State.Ready;

        /// <summary>
        /// Initializes a new instance of the <see cref="Controller"/> class.
        /// </summary>
        /// <param name="worldModel">The world model.</param>
        public Controller(WorldModel worldModel)
        {
            m_worldModel = worldModel;
        }

        /// <summary>
        /// The m execute thread
        /// </summary>
        private Thread m_execThread;
        /// <summary>
        /// Occurs when [state changed].
        /// </summary>
        public event Action<State> StateChanged;

        /// <summary>
        /// Plays the pause.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cannot start, pause or resume from the STOPPED state.</exception>
        public void PlayPause()
        {
            switch (ModelState)
            {
                case State.Ready:
                    InitializeExecutive(m_worldModel.ExecutionParameters);
                    ModelState = State.Running;
                    StateChanged?.Invoke(ModelState);
                    m_execThread = new Thread(m_worldModel.Executive.Start);

                    try
                    {
                        m_execThread.Start();
                    }
                    catch (ThreadAbortException)
                    {
                        
                    }
                    break;
                case State.Running:
                    m_worldModel.Executive.Pause();
                    ModelState = State.Paused;
                    StateChanged?.Invoke(ModelState);
                    break;
                case State.Paused:
                    m_worldModel.Executive.Resume();
                    ModelState = State.Running;
                    StateChanged?.Invoke(ModelState);
                    break;
                case State.Stopped:
                    throw new InvalidOperationException("Cannot start, pause or resume from the STOPPED state.");
                    
                default:
                    break;
            }
        }

        /// <summary>
        /// The m first initialization
        /// </summary>
        private bool m_firstInitialization = true;
        /// <summary>
        /// Initializes the executive.
        /// </summary>
        /// <param name="executionParameters">The execution parameters.</param>
        private void InitializeExecutive(ExecParameters executionParameters)
        {
            m_worldModel.Executive.SetStartTime(executionParameters.StartTime);
            m_worldModel.Executive.RequestEvent((exec, data) => exec.Stop(), executionParameters.EndTime);
            Metronome_Simple metronome = Metronome_Simple.CreateMetronome(
                m_worldModel.Executive,
                executionParameters.StartTime,
                executionParameters.EndTime,
                executionParameters.Increment);
            if (m_firstInitialization)
            {
                metronome.TickEvent += m_worldModel.Update;
            }
            m_firstInitialization = false;
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Cannot stop from the READY state.
        /// or
        /// Cannot stop from the STOPPED state.
        /// </exception>
        public void Stop()
        {
            switch (ModelState)
            {
                case State.Ready:
                    throw new InvalidOperationException("Cannot stop from the READY state.");
                case State.Running:
                    m_worldModel.Executive.Stop();
                    ModelState = State.Stopped;
                    StateChanged?.Invoke(ModelState);
                    break;
                case State.Paused:
                    m_worldModel.Executive.Stop();
                    ModelState = State.Stopped;
                    StateChanged?.Invoke(ModelState);
                    break;
                case State.Stopped:
                    throw new InvalidOperationException("Cannot stop from the STOPPED state.");
            }
        }
        /// <summary>
        /// Resets this instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cannot stop from the READY state.</exception>
        public void Reset()
        {
            switch (ModelState)
            {
                case State.Ready:
                    throw new InvalidOperationException("Cannot stop from the READY state.");
                case State.Running:
                    Stop();
                    m_worldModel.Executive.Reset();
                    ModelState = State.Ready;
                    StateChanged?.Invoke(ModelState);
                    break;
                case State.Paused:
                    Stop();
                    m_worldModel.Executive.Reset();
                    ModelState = State.Ready;
                    StateChanged?.Invoke(ModelState);
                    break;
                case State.Stopped:
                    m_worldModel.Executive.Reset();
                    ModelState = State.Ready;
                    StateChanged?.Invoke(ModelState);
                    break;
            }
        }
    }

}
