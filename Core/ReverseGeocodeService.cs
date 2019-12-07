using Wibci.CountryReverseGeocode;
using Wibci.CountryReverseGeocode.Models;

namespace Core
{
    public static class ReverseGeocodeService
    {
        private static CountryReverseGeocodeService m_crgs = new CountryReverseGeocodeService();


        public static string CountryCodeForLatAndLong(double lat, double lon)
        {
            return m_crgs.FindCountry(new GeoLocation() { Latitude = lat, Longitude = lon })?.Id??"--";
        }

        public static string CountryNameForLatAndLong(double lat, double lon)
        {
            return m_crgs.FindCountry(new GeoLocation() { Latitude = lat, Longitude = lon })?.Name??"--";
        }
    }
}
