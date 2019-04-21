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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Highpoint.Sage.SimCore;
using Highpoint.Sage.SystemDynamics;

namespace Core
{
    /// <summary>
    /// Class WorldModel.
    /// </summary>
    public class WorldModel
    {

        public GlobalAirTravelPolicy GlobalAirTravelPolicy = new GlobalAirTravelPolicy();

        /// <summary>
        /// The m data
        /// </summary>
        private MapData m_mapData;
        /// <summary>
        /// The m data
        /// </summary>
        private List<CountryData> m_countryData;
        /// <summary>
        /// The m nodes
        /// </summary>
        private DiseaseNode[,] m_nodes;
        /// <summary>
        /// The m size
        /// </summary>
        private Size m_size;

        /// <summary>
        /// The m controller
        /// </summary>
        private Controller m_controller;

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
        /// <param name="execParameters">The execute parameters.</param>
        public WorldModel(MapData mapData, List<CountryData> countryData, ExecParameters execParameters = null)
        {
            Executive = ExecFactory.Instance.CreateExecutive(ExecType.FullFeatured);
            ExecutionParameters = execParameters?? new ExecParameters();
            m_controller = new Controller(this);

            m_countryData = countryData;
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

        private List<NationalGovernment> m_governments = new List<NationalGovernment>();
        public List<NationalGovernment> Governments => m_governments; 
        private void CreateAndAssignGovernments()
        {

            Dictionary<string, List<DiseaseNode>> nodeToCountryMapping = new Dictionary<string, List<DiseaseNode>>();

            foreach (DiseaseNode diseaseNode in m_nodes)
            {
                string cc = diseaseNode.MapCell.CountryCode;
                if ( cc != null && !"--".Equals(cc) )
                {
                    List<DiseaseNode> diseaseNodes;
                    if (!nodeToCountryMapping.TryGetValue(cc, out diseaseNodes))
                    {
                        diseaseNodes = new List<DiseaseNode>();
                        nodeToCountryMapping.Add(cc, diseaseNodes);
                    }
                    diseaseNodes.Add(diseaseNode);
                }
            }

            foreach (CountryData countryData in CountryData)
            {
                if (nodeToCountryMapping.ContainsKey(countryData.CountryCode))
                {
                    NationalGovernment ng = new NationalGovernment()
                    {
                        CountryData = countryData,
                        DiseaseNodes = nodeToCountryMapping[countryData.CountryCode]
                    };
                    countryData.Government = ng;
                    m_governments.Add(ng);
                }
                else
                {
                    Console.WriteLine($"Could not find a list of nodes for country code {countryData.CountryCode}");
                }
            }
        }

        /// <summary>
        /// Gets the map data.
        /// </summary>
        /// <value>The map data.</value>
        public MapData MapData => m_mapData;

        /// <summary>
        /// Gets the map data.
        /// </summary>
        /// <value>The map data.</value>
        public List<CountryData> CountryData => m_countryData;

        /// <summary>
        /// Announces when a new iteration is available.
        /// </summary>
        public event Action<DiseaseNode[,], double[], List<RouteData>> NewIterationAvailable;

        /// <summary>
        /// Runs the specified manual reset event1.
        /// </summary>
        /// <param name="manualResetEvent1">The manual reset event1.</param>
        /// <param name="manualResetEvent2">The manual reset event2.</param>
        public void Run(ManualResetEvent manualResetEvent1, ManualResetEvent manualResetEvent2)
        {
            while (m_nodes[0, 0].Start + (m_nodes[0, 0].TimeSliceNdx * m_nodes[0, 0].TimeStep) < m_nodes[0, 0].Finish)
            {
                Update(manualResetEvent1, manualResetEvent2);
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
            Update();
        }

        /// <summary>
        /// Updates the specified manual reset event1.
        /// </summary>
        /// <param name="manualResetEvent1">The manual reset event1.</param>
        /// <param name="manualResetEvent2">The manual reset event2.</param>
        internal void Update(ManualResetEvent manualResetEvent1 = null, ManualResetEvent manualResetEvent2 = null)
        {
            bool travelOnlyFromToApparentlyDiseaseFree = NoAirTravelFromToKnownInfected;
            manualResetEvent1?.WaitOne();
            manualResetEvent2?.WaitOne();
            //DateTime now = DateTime.Now;
            DiseaseNode[,] newNodes = new DiseaseNode[m_mapData.Width, m_mapData.Height];
            // Progress the simulation one timestep.

            int count1 = 0, count2 = 0;

#if AUTO_INOCULATE
            if (m_nodes[0, 0].TimeSliceNdx == 10 && m_districtsOfInterest != null)
            {
                {
                    foreach (var tuple in m_districtsOfInterest)
                    {
                        Console.WriteLine($"Inoculating at {tuple.Item1},{tuple.Item2} with patient zero.");
                        m_nodes[tuple.Item1, tuple.Item2].ContagiousAsymptomatic++;
                        Console.WriteLine(m_nodes[tuple.Item1, tuple.Item2]);
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
                        newNodes[x, y] = m_nodes[x, y];
                        newNodes[x, y].TimeSliceNdx++;
                        Interlocked.Increment(ref count1);
                    }
                    else
                    {
                        //EpidemicNode.m_debug = (x == 243 && y == 240);
                        //if ( x== 243 && y == 240 && m_nodes[x, y].TimeSliceNdx == 63) Debugger.Break();
                        newNodes[x, y] = Behavior<DiseaseNode>.RunOneTimeslice(m_nodes[x, y]);
                        Interlocked.Increment(ref count2);
                    }
                }

#if USE_PARALLEL
            });
#else
            } // Non-parallel.
#endif
            //Console.WriteLine($"New state computation takes {(DateTime.Now - now).TotalMilliseconds} mSec.");
            //now = DateTime.Now;

            // Now propagate contagiouses to neighbors.
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
                                    double newInfections = Math.Min(traveling*neighbor.ContractionRate,
                                        neighbor.Susceptible);
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

            //Console.WriteLine($"Neighbor propagation takes {(DateTime.Now - now).TotalMilliseconds} mSec.");
            //now = DateTime.Now;

            double[] routeTransmittals = new double[0];
#if ACCOMODATE_AIR_TRAVEL
            // Now process Air Travel.
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
            m_nodes = newNodes;
            NewIterationAvailable?.Invoke(newNodes, routeTransmittals, BusyRoutes);
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

        public CountryData CountryForCode(string countryCode)
        {
            // TODO: Make this a dictionary.
            return m_countryData.FirstOrDefault(data => data.CountryCode.Equals(countryCode));
        }
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
                    catch (ThreadAbortException tae)
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
