using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider
{
    internal class DatabaseService
    {
        private readonly IDriver driver;

        public DatabaseService()
        {
            this.driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "password"));
        }

        public Client GetClientByName(string firstName, string lastName)
        {
            using (var session = driver.Session())
            {
                Client client = session.ReadTransaction(tx =>
                {
                    var result = tx.Run("" +
                        "MATCH (a:Client) " +
                        "WHERE a.firstName = $firstName " +
                        "AND a.lastName = $lastName " +
                        "RETURN a.firstName, a.lastName, a.placeOfBirth, a.address",
                        new { firstName, lastName });
                    
                    var record = result.First();
                    return new Client()
                    {
                        firstName = record["a.firstName"].ToString(),
                        lastName = record["a.lastName"].ToString(),
                        placeOfBirth = record["a.placeOfBirth"].ToString(),
                        address = record["a.address"].ToString(),
                    };
                });
                return client;
            }
        }

        public List<string> GetCities(string name)
        {
            using (var session = driver.Session())
            {
                List<string> cities = session.ReadTransaction(tx =>
                {
                    var result = tx.Run("" +
                        "MATCH (a:City) " +
                        "WHERE toLower(a.name) CONTAINS toLower($name) " +
                        "RETURN a.name",
                        new { name });

                    List<string> fetchedCities = new List<string>();

                    foreach (var record in result)
                    {
                        fetchedCities.Add(record["a.name"].ToString());
                    }

                    return fetchedCities;
                });
                return cities;
            }
        }

        public bool Login(string username, string password)
        {
            using (var session = driver.Session())
            {
                List<string> passList = session.ReadTransaction(tx =>
                {
                    var result = tx.Run("" +
                        "MATCH (a: Client) " +
                        "WHERE a.email = $username " +
                        "RETURN a.password",
                        new {username});
                    List<string> passwords = new List<string>();
                    foreach(var record in result)
                    {
                        passwords.Add(record["a.password"].ToString());
                    }
                    return passwords;
                });
                if (passList.Count != 1)
                {
                    return false;
                }
                else
                {
                    return passList[0] == password;
                }
            }
        }

        public void Register(string firstName, string lastName, string gender, 
                             string email, string address, string dob, 
                             string placeOfBirth, long pin, string password) 
        {
            using (var session = driver.Session())
            {
                var result = session.WriteTransaction(tx =>
                {
                    var query_result = tx.Run("" +
                        "CREATE (c:Client {firstName: $firstName, lastName: $lastName, " +
                        "gender: $gender, email: $email, address: $address, " +
                        "dateOfBirth: $dob, placeOfBirth: $placeOfBirth, pin: $pin, " +
                        "password: $password" +
                        "})", 
                        new { firstName, lastName, gender, email, 
                              address, dob, placeOfBirth, pin, password
                        });

                    var summary = query_result.Consume();
                    Console.WriteLine(summary);
                    return query_result;
                });
                return;
            }
        }

        public bool ValidateEmail(string email) {
            using (var session = driver.Session())
            {
                bool emailExists = session.ReadTransaction(tx =>
                {
                    var result = tx.Run("" +
                        "MATCH (c:Client) " +
                        "WHERE c.email = $email " +
                        "RETURN c.email;",
                        new { email });
                    return result.Any();
                });
                if (emailExists) return false;
                return true;
            }
        }
    }
}
    