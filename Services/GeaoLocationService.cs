using MaxMind.GeoIP2;
using System.Net;

namespace Kill_hunger.Services
{
    public static class GeaoLocationService
    {
        public static List<String> GetIpLocation(String ip)
        {
            List<String> GealocationData = new List<string>();
            IPAddress address = IPAddress.Parse(ip);

            if (IPAddress.IsLoopback(address))
            {
                return null;
            }

            using (var reader = new DatabaseReader(Directory.GetCurrentDirectory() + "/GeoLite2-City.mmdb"))
            {
                var response = reader.City(ip);
                var responsecountry = reader.Country(ip);

                GealocationData.Add(response.City.Name ?? "");

                GealocationData.Add(responsecountry.Country.Name ?? "");
            }

            return GealocationData;
        }
    }
}
