using System;
using System.Drawing;
using Highpoint.Sage.SimCore;

namespace Core
{
    public class OutbreakResponseTeam
    {
        private static readonly double EPSILON = 1E-6;
        private readonly string m_cc;
        private readonly int m_ordinal;
        private readonly Color m_color;
        private readonly string m_name;
        private readonly WorldModel m_model;
        private Coordinates m_coordinates;
        private Coordinates m_destination;
        private bool m_standingBy;
        private double m_movementSpeed;
        private DateTime m_lastPositionUpdate = DateTime.MinValue;
        private static double REPLAN_PERIOD_IN_HOURS = 24.0;
        public const double AIRCRAFT_MOVE_SPEED = 500.0;
        public const double CONVOY_MOVE_SPEED = 5.0;
        private readonly ILineWriter console = Locator.Resolve<ILineWriter>("console");
        

        private int LOCAL_MOVE_RANGE = 24;

        public OutbreakResponseTeam(string cc, int ordinal, Color color, WorldModel model, Coordinates initialCoordinates)
        {
            m_cc = cc;
            m_ordinal = ordinal;
            m_color = color;
            m_model = model;
            m_coordinates = initialCoordinates;
            m_destination = m_coordinates;
            m_movementSpeed = AIRCRAFT_MOVE_SPEED;
            m_standingBy = true;  // Becomes false once first governmental assignment has been made.
            m_name = $"ORT-{m_cc}({m_ordinal})";
            
            console.WriteLine($"Initialized {m_name} in {m_color} at {m_coordinates}.");

        }

        public void SetDestinationAndSpeed(Coordinates destination, double speed)
        {
            m_destination = destination;
            m_movementSpeed = speed;
            m_standingBy = false;
            console.WriteLine($"Directing {m_name} to proceed at speed {speed} to {destination}.");
            m_lastPositionUpdate = m_model.Executive.Now;
            m_model.Executive.RequestEvent(UpdatePosition, m_model.Executive.Now + TimeSpan.FromHours(3.0));
        }

        private void UpdatePosition(IExecutive exec, object userData)
        {
            TimeSpan elapsed = exec.Now - m_lastPositionUpdate;
            double distanceRemaining = m_coordinates.DistanceTo(m_destination);
            double distanceTraveled = m_movementSpeed * elapsed.TotalDays;
            if (distanceRemaining - distanceTraveled > EPSILON)
            {
                // We aren't there yet. Impart progress toward destination.
                double fraction = distanceTraveled/distanceRemaining;
                m_coordinates.X += (int)Math.Round(fraction * (m_destination.X - m_coordinates.X));
                m_coordinates.Y += (int)Math.Round(fraction * (m_destination.Y - m_coordinates.Y));
                m_model.Executive.RequestEvent(UpdatePosition, m_model.Executive.Now + TimeSpan.FromHours(3.0));
            }
            else
            {
                m_coordinates = m_destination;
                // We've arrived.
                double hoursToNextReplan = REPLAN_PERIOD_IN_HOURS;
                DateTime when = m_model.Executive.Now + TimeSpan.FromHours(hoursToNextReplan);
                m_model.Executive.RequestDaemonEvent(Update, when, 0.0, null);
            }
        }

        public Color Color => m_color;
        public bool StandingBy => m_standingBy;
        public Coordinates Location => m_coordinates;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exec"></param>
        /// <param name="data"></param>
        private void Update(IExecutive exec, object data)
        {
            if (m_standingBy) return;
            if (!m_coordinates.Equals(m_destination)) return; // We're still traveling.


            // The specifics of ORT planning are not yet determined, at least in terms of what an ORT's
            // strategy might best be in the real world. But the logic here is intended as a placeholder
            // for such logic.
            //
            // Any ORT in a cell will increase Immunized and Recovery flows by 25%.
            // 
            // It chooses where to go by testing all cells within x= +/- LOCAL_MOVE_RANGE, and 
            // y = +/- LOCAL_MOVE_RANGE, creating a Sqrt(Immunized^2 + Recovery^2) metric and the 
            // largest value in a cell that has a value more than 25% higher than that of the present
            // cell becomes a migration target without cost.
            //
            // When it decides to move, an ORT can move 5 cells a day.

            //console.WriteLine($"ORT {m_name} is planning its actions for {m_model.Executive.Now}.");


            Func<DiseaseNode, double> fitness =
                dn => Math.Pow(dn.ImmunizationRate, 2) + Math.Pow(dn.RecoveryRate, 2);

            double fitnessOfHere = fitness(m_model.DiseaseNodes[m_coordinates.X, m_coordinates.Y])/1.25;
            Tuple<int, int, double> best = new Tuple<int, int, double>(m_coordinates.X, m_coordinates.Y, fitnessOfHere);


            for (int xCursor = m_coordinates.X - LOCAL_MOVE_RANGE;
                xCursor < m_coordinates.X + LOCAL_MOVE_RANGE;
                xCursor++)
            {
                for (int yCursor = m_coordinates.Y - LOCAL_MOVE_RANGE;
                    yCursor < m_coordinates.Y + LOCAL_MOVE_RANGE;
                    yCursor++)
                {
                    if (xCursor == m_coordinates.X && yCursor == m_coordinates.Y) continue;
                    int xTarget = xCursor > 0
                        ? xCursor < m_model.Size.Width ? xCursor : xCursor - m_model.Size.Width
                        : xCursor + m_model.Size.Width;
                    int yTarget = yCursor > 0
                        ? yCursor < m_model.Size.Height ? yCursor : yCursor - m_model.Size.Height
                        : yCursor + m_model.Size.Height;

                    fitnessOfHere = fitness(m_model.DiseaseNodes[xTarget, yTarget]);
                    if (fitnessOfHere > best.Item3) best = new Tuple<int, int, double>(xTarget, yTarget, fitnessOfHere);
                }
            }

            // If there's a more fit node within +/- LOCAL_MOVE_RANGE, start moving there by convoy.
            if (fitnessOfHere - best.Item3 < EPSILON)
            {
                /////////////////////////////////////////////////////////////////////////////
                // Institute convoy movement 
                // 
                m_destination.X = best.Item1;
                m_destination.Y = best.Item2;
                //
                /////////////////////////////////////////////////////////////////////////////

                return;
            }

            DateTime nextUpdate = m_model.Executive.Now + TimeSpan.FromDays(1.0);
            m_model.Executive.RequestEvent(Update, nextUpdate);
        }

        public override string ToString()
        {
            return m_name;
        }
    }
}
