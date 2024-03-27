namespace DymapticHomework
{
    public class Person
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool? IsFemale { get; set; }
        public bool? IsStudent { get; set; }
        public bool? IsEmployee { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Person(string name, int? age, string street, string city, string state, string flags, double latitude, double longitude)
        {
            Name = name;
            Age = age;
            Street = street;
            City = city;
            State = state;
            if(!string.IsNullOrEmpty(flags)) 
            {
                IsFemale = flags.ToUpper()[0] == 'Y';
                IsStudent = flags.ToUpper()[1] == 'Y';
                IsEmployee = flags.ToUpper()[2] == 'Y';
            } else
            {
                IsFemale = null;
                IsStudent = null;
                IsEmployee = null;
            }
            Latitude = latitude;
            Longitude = longitude;
        }

        public override string ToString()
        {
            string gender = IsFemale.HasValue ? (IsFemale.Value ? "Female" : "Male") : "Unknown";
            string? ageString = Age.HasValue ? Age.ToString() : "";
            string studentStatus = IsStudent.HasValue ? (IsStudent.Value ? "Yes" : "No") : "Unknown";
            string employeeStatus = IsEmployee.HasValue ? (IsEmployee.Value ? "Yes" : "No") : "Unknown";

            // Check if latitude and longitude are 0.0, and display "Unknown" in that case
            string latitudeString = Latitude != 0.0 ? Latitude.ToString() : "Unknown";
            string longitudeString = Longitude != 0.0 ? Longitude.ToString() : "Unknown";

            string output = $"{Name} {(Age.HasValue ? $"[{ageString}, {gender}]" : $"[{gender}]")}\n";
            output += $"   Street   : {Street}\n";
            output += $"   City     : {City}\n";
            output += $"   State    : {(string.IsNullOrEmpty(State) ? "N/A" : State)}\n";
            output += $"   Student  : {studentStatus}\n";
            output += $"   Employee : {employeeStatus}\n";
            output += $"   Latitude/Longitude: {latitudeString}, {longitudeString}\n";
            output += $"------------------------------------------------------\n";

            return output;
        }
    }
}
