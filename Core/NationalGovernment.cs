using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class NationalGovernment
    {

        public string Name => CountryData?.Country ?? "Unknown";

        internal NationalGovernment() { /*For CSVHelper deserialization only.*/ }

        //public NationalGovernment(CountryData countryData, List<DiseaseNode> listOfDiseaseNodes)
        //{
        //    CountryData = countryData;
        //    countryData.Government = this;
        //    DiseaseNodes = listOfDiseaseNodes;
        //}

        public List<DiseaseNode> DiseaseNodes { get; internal set; }

        public CountryData CountryData { get; internal set; }

        public List<Policy> Policies { get; internal set; }
        public override string ToString()
        {
            return $"Government of {CountryData?.Country??"???"}";
        }
    }
}
