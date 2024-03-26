using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace DymapticHomework
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Edit FilePath or people.txt to run with different test data
            string solutionDirectory = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory))));
            string filePath = Path.Combine(solutionDirectory, "people.txt");
            string apiKey = "6602e9821e46a466100770cmd8f4fb1";

            Console.WriteLine("This will take approximately 1 second for each person record");

            string dymapticAddress = "811 SW 6th Ave, Portland, OR 97204";
            double dymapticLatitude = 0.0;
            double dymapticLongitude = 0.0;
            GetGeocode(dymapticAddress, apiKey, out dymapticLatitude, out dymapticLongitude);

            List<Person> peopleList = CreatePeopleList(filePath, apiKey);

            Console.WriteLine();
            
            // Printing out the people to the Console
            foreach (Person person in peopleList)
            {
                Console.WriteLine(person.ToString());
            }

            Console.WriteLine("The person living closest to Dymaptic Headquarters is: ");
            Console.WriteLine(FindClosestPerson(dymapticLatitude, dymapticLongitude, peopleList).ToString());
        }

        static List<Person> CreatePeopleList(string filepath, string apiKey)
        {
            string line;
            using StreamReader streamReader = new StreamReader(filepath);
            List<Person> peopleList = new List<Person>();
            string name = "";
            int? age = null;
            string street = "";
            string city = "";
            string state = "";
            string flags = "";
            double latitude = 0.0;
            double longitude = 0.0;

            while ((line = streamReader.ReadLine()) != null)
            {
                if (line.Trim() == "")
                {
                    // If blank line is encountered and we have a person to create, Add person
                    if (!string.IsNullOrEmpty(name))
                    {
                        GetGeocode($"{street}+{city}+{state}", apiKey, out latitude, out longitude);
                        Person person = new(name, age, street, city, state, flags, latitude, longitude);
                        peopleList.Add(person);
                    }

                    // Reset values for the next person
                    name = "";
                    age = null;
                    street = "";
                    city = "";
                    state = "";
                    flags = "";
                }
                else
                {
                    string[] keyValue = line.Split(')');
                    if (keyValue.Length >= 2) // Check if keyValue has at least two elements
                    {
                        string key = keyValue[0].TrimStart('(');
                        string value = keyValue[1].Trim();

                        switch (key.ToLower())
                        {
                            case "name":
                                name = value;
                                break;
                            case "age":
                                age = int.Parse(value);
                                break;
                            case "street":
                                street = value;
                                break;
                            case "city":
                                if (value.Contains(','))
                                {
                                    string[] cityState = value.Split(',');
                                    city = cityState[0].Trim();
                                    state = cityState[1].Trim();
                                }
                                else
                                {
                                    city = value;
                                }
                                break;
                            case "flags":
                                flags = value;
                                break;
                        }
                    }
                }
            }
            // Add last person if needed
            if (!string.IsNullOrEmpty(name))
            {
                GetGeocode($"{street}+{city}+{state}", apiKey, out latitude, out longitude);
                Person person = new(name, age, street, city, state, flags, latitude, longitude);
                peopleList.Add(person);
            }
            return peopleList;
        }

        static void GetGeocode(string address, string apiKey, out double latitude, out double longitude)
        {
            Console.Write("."); // Write to console for better user experience
            latitude = 0.0;
            longitude = 0.0;

            string apiUrl = $"https://geocode.maps.co/search?q={WebUtility.UrlEncode(address)}&api_key={apiKey}";
            Thread.Sleep(1000);
            try
            {
                using (WebClient client = new WebClient())
                {
                    string response = client.DownloadString(apiUrl);
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response);

                    // Check if the JSON response is an array
                    if (jsonResponse is JArray jsonArray)
                    {
                        // If the array contains objects, assume the first object contains latitude and longitude
                        if (jsonArray.Count > 0 && jsonArray[0]["lat"] != null && jsonArray[0]["lon"] != null)
                        {
                            latitude = double.Parse(jsonArray[0]["lat"].ToString());
                            longitude = double.Parse(jsonArray[0]["lon"].ToString());
                        }
                        else
                        {
                            Console.WriteLine("No latitude and longitude found for the given address.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Unexpected JSON response format.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving geocode: {ex.Message}");
            }
        }

        static Person FindClosestPerson(double dymapticLatitude, double dymapticLongitude, List<Person> peopleList)
        {
            Person closestPerson = null;
            double minDistance = double.MaxValue;
            
            foreach (Person person in peopleList) 
            {
                double distance = CalculateDistance(dymapticLatitude, dymapticLongitude, person.Latitude, person.Longitude);
                if (distance < minDistance) 
                {
                    minDistance = distance;
                    closestPerson = person;
                }
            }

            return closestPerson;
        }

        static double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            
            const double earthRadius = 6371; // kilometers

            // Convert latitude and longitude from degrees to radians
            double dLat = (latitude2 - latitude1) * (Math.PI / 180);
            double dLon = (longitude2 - longitude1) * (Math.PI / 180);

            // Haversine formula used to calculate distance with latitude/longitude
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(latitude1 * (Math.PI / 180)) * Math.Cos(latitude2 * (Math.PI / 180)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = earthRadius * c;

            return distance;
        }
    }
}
