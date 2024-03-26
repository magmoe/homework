using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DymapticHomework
{
    public class Person
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; } = "N/A";
        public bool IsFemale { get; set; }
        public bool IsStudent { get; set; }
        public bool IsEmployee { get; set; }
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
            }
            Latitude = latitude;
            Longitude = longitude;
        }

        public override string ToString()
        {
            string gender = IsFemale ? "Female" : "Male";
            string ageString = Age.HasValue ? Age.ToString() : "";
            string studentStatus = IsStudent ? "Yes" : "No";
            string employeeStatus = IsEmployee ? "Yes" : "No";

            string output = $"{Name} {(Age.HasValue ? $"[{ageString}, {gender}]" : $"[{gender}]")}\n";
            output += $"   Street   : {Street}\n";
            output += $"   City     : {City}\n";
            output += $"   State    : {(string.IsNullOrEmpty(State) ? "N/A" : State)}\n";
            output += $"   Student  : {studentStatus}\n";
            output += $"   Employee : {employeeStatus}\n";
            output += $"   Latitude/Longitude {Latitude}, {Longitude}\n";

            return output;
        }
    }
}
