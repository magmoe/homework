namespace DymapticHomework
{
    public class Program
    {
        const string apiKey = "6602e9821e46a466100770cmd8f4fb1";
        private const string dymapticAddress = "811 SW 6th Ave, Portland, OR 97204";
        private const string separator = "------------------------------------------------------";

        public static void Main(string[] args)
        {
            // Edit people.txt or use a different file here to run with different test data
            string filePath = FileUtils.GetSolutionDirectoryFilePath("people.txt");

            // Make a list of all the people and their info
            List<Person> peopleList = FileUtils.LoadPeopleFromFile(filePath, apiKey);

            // Get latitude/longitude of Dymaptic Headquarters
            (double dymapticLatitude, double dymapticLongitude) = GeocodingService.GetGeocode(dymapticAddress, apiKey);
            // Printing out the people to the Console
            PrintPeople(peopleList);
            PrintClosestPerson(dymapticLatitude, dymapticLongitude, peopleList);
        }

        private static void PrintPeople(List<Person> peopleList)
        {
            Console.WriteLine();
            Console.WriteLine("Records for " +  peopleList.Count + " people");
            Console.WriteLine(separator);
            foreach (Person person in peopleList)
            {
                Console.WriteLine(person.ToString());
            }
        }

        private static void PrintClosestPerson(double dymapticLatitude, double dymapticLongitude, List<Person> peopleList) 
        {
            Person closestPerson = GeocodingService.FindClosestPerson(dymapticLatitude, dymapticLongitude, peopleList);
            Console.WriteLine("The person living closest to Dymaptic Headquarters is: ");
            Console.WriteLine(separator);
            Console.WriteLine((closestPerson != null) ? closestPerson.ToString() : "Unknown");
        } 
    }
}
