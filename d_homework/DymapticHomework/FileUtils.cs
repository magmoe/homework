namespace DymapticHomework
{
    public static class FileUtils
    {
        public static string GetSolutionDirectoryFilePath(string fileName)
        {
            string solutionDirectory = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory))));
            return Path.Combine(solutionDirectory, fileName);
        }

        public static List<Person> LoadPeopleFromFile(string filePath, string apiKey)
        {
            Console.WriteLine("Loading will take approximately 1 second per person record");
            List<Person> peopleList = new List<Person>();

            try
            {
                string name = "";
                int? age = null;
                string street = "";
                string city = "";
                string state = "";
                string flags = "";
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();

                        if (string.IsNullOrWhiteSpace(line))
                        {
                            // Check to see if person data needs added
                            if (!string.IsNullOrEmpty(name))
                            {
                                AddPersonToList(ref peopleList, name, age, street, city, state, flags, apiKey);
                                ResetPersonData(ref name, ref age, ref street, ref city, ref state, ref flags);
                            }
                        }
                        else
                        {
                            ParseKeyValuePair(line, ref name, ref age, ref street, ref city, ref state, ref flags);
                        }
                    }

                    // Add the last person if needed
                    if (!string.IsNullOrEmpty(name))
                    {
                        AddPersonToList(ref peopleList, name, age, street, city, state, flags, apiKey);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading people from file: {ex.Message}");
            }

            return peopleList;
        }

        private static void AddPersonToList(ref List<Person> peopleList, string name, int? age, string street, string city, string state, string flags, string apiKey)
        {
            (double latitude, double longitude) = GeocodingService.GetGeocode($"{street}+{city}+{state}", apiKey);
            Person person = new(name, age, street, city, state, flags, latitude, longitude);
            peopleList.Add(person);
        }

        private static void ResetPersonData(ref string name, ref int? age, ref string street, ref string city, ref string state, ref string flags)
        {
            name = "";
            age = null;
            street = "";
            city = "";
            state = "";
            flags = "";
        }

        private static void ParseKeyValuePair(string line, ref string name, ref int? age, ref string street, ref string city, ref string state, ref string flags)
        {
            string[] keyValue = line.Split(')');
            if (keyValue.Length >= 2)
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
                    default:
                        Console.WriteLine("Check Record for " + name + ". Unknown key: " + key + " Value: " + value + " This data will not be included.");
                        break;
                }
            }
        }
    }
}
