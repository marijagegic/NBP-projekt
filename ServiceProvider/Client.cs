using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider
{
    internal class Client
    {
        public string lastName;
        public string firstName;
        public string country;
        public string placeOfBirth;
        public string address;
        public string gender;
        public double pin;
        public string email;
        public DateTime dateOfBirth;

        public Client() { }
        public Client(string lastName, string firstName, string country, string placeOfBirth, 
            string address, string gender, double pin, string email, DateTime dateOfBirth)
        {
            this.lastName = lastName;
            this.firstName = firstName;
            this.country = country;
            this.placeOfBirth = placeOfBirth;
            this.address = address;
            this.gender = gender;
            this.pin = pin;
            this.email = email;
            this.dateOfBirth = dateOfBirth;
        }


    }
}
