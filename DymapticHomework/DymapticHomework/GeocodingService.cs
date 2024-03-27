using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;

namespace DymapticHomework
{
    public static class GeocodingService
    {
        public static (double latitude, double longitude) GetGeocode(string address, string apiKey)
        {
            Console.Write("."); // Write to console for better user experience

            string apiUrl = $"https://geocode.maps.co/search?q={WebUtility.UrlEncode(address)}&api_key={apiKey}";
            Thread.Sleep(1000);
            try
            {
                using (WebClient client = new WebClient())
                {
                    string response = client.DownloadString(apiUrl);
                    var jsonResponse = JsonConvert.DeserializeObject(response);

                    // Check if the JSON response is an array
                    if (jsonResponse is JArray jsonArray && jsonArray.Count > 0)
                    {
                        // If the array contains objects, assume the first object contains latitude and longitude
                        if (jsonArray[0]["lat"] != null && jsonArray[0]["lon"] != null)
                        {
                            double latitude = double.Parse(jsonArray[0]["lat"].ToString());
                            double longitude = double.Parse(jsonArray[0]["lon"].ToString());
                            return (latitude, longitude);
                        }
                    }

                    throw new Exception("No latitude and longitude found for " + address);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving geocode: {ex.Message}");
                return (0.0, 0.0); 
            }
        }

        public static Person FindClosestPerson(double dymapticLatitude, double dymapticLongitude, List<Person> peopleList)
        {
            Person closestPerson = null;
            double minDistance = double.MaxValue;

            foreach (Person person in peopleList)
            {
                if(person.Latitude != 0 && person.Longitude != 0)
                {
                    double distance = CalculateDistance(dymapticLatitude, dymapticLongitude, person.Latitude, person.Longitude);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestPerson = person;
                    }
                }
            }
            return closestPerson;
        }

        public static double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {

            const double earthRadiusKm = 6371;

            // Convert latitude and longitude from degrees to radians
            double lat1Rad = latitude1 * (Math.PI / 180);
            double lon1Rad = longitude1 * (Math.PI / 180);
            double lat2Rad = latitude2 * (Math.PI / 180);
            double lon2Rad = longitude2 * (Math.PI / 180);

            // Calculate differences in latitude and longitude
            double deltaLat = lat2Rad - lat1Rad;
            double deltaLon = lon2Rad - lon1Rad;

            // Haversine formula used to calculate distance with latitude/longitude
            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) + Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = earthRadiusKm * c;

            return distance;
        }
    }
}
