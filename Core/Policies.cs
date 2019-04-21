using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IPolicy
    {
        string Name { get; }
        string Scope { get; }
    }

    public class GlobalAirTravelPolicy : AirTravelPolicy
    {
        public override string Scope => "Global";
    }

    public class AirTravelPolicy : IPolicy
    {
        public enum _Restrictions { Unrestricted = 0, OnlyCleanToClean = 1, Grounded = 2 }        
        public string Name => "Flight Restrictions";
        public virtual string Scope { get; set; }
        public _Restrictions Restrictions = _Restrictions.Unrestricted;
        public string RestrictionText => m_restrictionText[(int)Restrictions];
        private string[] m_restrictionText = new[]
        {"Unrestricted Air travel", "Air travel between uninfected locales only", "No air travel"};
    }
}
