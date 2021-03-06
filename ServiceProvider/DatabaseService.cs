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
                        new { username });
                    List<string> passwords = new List<string>();
                    foreach (var record in result)
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
                        "dateOfBirth: date($dob), placeOfBirth: $placeOfBirth, pin: $pin, " +
                        "password: $password" +
                        "})",
                        new
                        {
                            firstName,
                            lastName,
                            gender,
                            email,
                            address,
                            dob,
                            placeOfBirth,
                            pin,
                            password
                        });

                    var summary = query_result.Consume();
                    Console.WriteLine(summary);
                    return query_result;
                });
                return;
            }
        }

        public bool ValidateEmail(string email)
        {
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

        public List<Tuple<string, string, string>> GetHotelRecommendations(string email, string destination, int ageDiff, int distance, int limit)
        {
            using (var session = driver.Session())
            {
                List<Tuple<string, string, string>> hotels = session.ReadTransaction(tx =>
                {
                    var result = tx.Run("" +
                        "MATCH(curr_client: Client), (destination: City), " +
                        "(client: Client) -[RESERVED] - (r: Reservation) -[IN] - (h: Hotel) -[SITUATED_IN] - (city: City) " +
                        "WHERE curr_client.email = $email " +
                        "AND destination.name = $destination " +
                        "AND client.dateOfBirth > curr_client.dateOfBirth - Duration({ years: $ageDiff}) " +
                        "AND client.dateOfBirth < curr_client.dateOfBirth + Duration({ years: $ageDiff}) " +
                        "WITH point.distance(" +
                        "point({ longitude: destination.lon, latitude: destination.lat, crs: 'WGS-84'}), " +
                        "point({ longitude: h.lon, latitude: h.lat, crs: 'WGS-84'})) as dist, h as h, city as city " +
                        "WHERE dist < $distance " +
                        "RETURN h.name, count(*) as cnt, h.stars, city.name ORDER BY cnt DESC LIMIT $limit",
                        new { email, destination, ageDiff, distance, limit }
                        );

                    List<Tuple<string, string, string>> fetchedHotels = new List<Tuple<string, string, string>>();

                    foreach (var record in result)
                    {
                        Tuple<string, string, string> item = new Tuple<string, string, string>(record["h.name"].ToString(), record["h.stars"].ToString(), record["city.name"].ToString());
                        fetchedHotels.Add(item);

                    }

                    return fetchedHotels;
                });
                return hotels;
            }
        }

        public List<Tuple<string, string, string>> GetHotelRecommendationsForClient(string email, int ageDiff, int limit)
        {
            using (var session = driver.Session())
            {
                List<Tuple<string, string, string>> hotels = session.ReadTransaction(tx =>
               {

                   var result = tx.Run("" +
                       "MATCH(curr_client: Client), (destination: City), " +
                       "(client: Client) -[RESERVED] - (r: Reservation) -[IN] - (h: Hotel) -[SITUATED_IN] - (city: City)" +
                       "WHERE curr_client.email =  $email " +
                       "AND client.gender = curr_client.gender " +
                       "AND client.dateOfBirth > curr_client.dateOfBirth - Duration({ years: $ageDiff}) " +
                       "AND client.dateOfBirth < curr_client.dateOfBirth + Duration({ years: $ageDiff}) " +
                       "RETURN h.name, h.stars, city.name, count(*) as cnt ORDER BY cnt DESC LIMIT $limit",
                       new { email, ageDiff, limit }
                       );

                   List<Tuple<string, string, string>> fetchedHotels = new List<Tuple<string, string, string>>();

                   foreach (var record in result)
                   {
                       Tuple<string, string, string> item = new Tuple<string, string, string>(record["h.name"].ToString(), record["h.stars"].ToString(), record["city.name"].ToString());
                       fetchedHotels.Add(item);

                   }

                   return fetchedHotels;
               });
                return hotels;
            }
        }

        public List<Hotel> GetHotels(string city, DateTime dateFrom, DateTime dateUntil, int personNumber)
        {
            using (var session = driver.Session())
            {
                List<Hotel> hotels = session.ReadTransaction(tx =>
                {
                    var result = tx.Run("" +
                        "MATCH (c:City)-[:SITUATED_IN]-(h:Hotel)-[:OFFERS]->(r:Room), (z:Reservation) " +
                        "WHERE c.name = $city " +
                        "AND r.beds >= $personNumber " +
                        "AND (" +
                        "(NOT (r)-[:TAKE]->()) " +
                        "OR " +
                        "((r)-[:TAKE]->(z) AND (z.checkIn > $dateUntil OR z.checkOut < $dateFrom))" +
                        ") " +
                        "RETURN distinct h.name, h.stars, h.halfBoard, h.fullBoard, h.allInclusive",
                        new
                        {
                            city,
                            dateFrom,
                            dateUntil,
                            personNumber
                        });

                    /*
                    var result = tx.Run("" +
                        "MATCH (h:Hotel)-[:OFFERS]-(r:Room)-[t?:TAKE]-(z:Reservation) " +
                        "WHERE h.city = $city " +
                        "AND r.beds >= $personNumber " +
                        "AND (" +
                        "(t is null) " +
                        "OR " +
                        "(z.checkIn > dateUntil OR z.checkOut < dateFrom)" +
                        ") " +
                        "RETURN h.name, h.stars, h.half_board, h.full_board, h.all_inclusive",
                        new { city, dateFrom, dateUntil, personNumber });
                    */
                    List<Hotel> fetchedHotels = new List<Hotel>();
                    foreach (var record in result)
                    {
                        Hotel h = new Hotel()
                        {
                            name = record["h.name"].ToString(),
                            stars = record["h.stars"].ToString(),
                            halfBoard = record["h.halfBoard"].ToString(),
                            fullBoard = record["h.fullBoard"].ToString(),
                            allInclusive = record["h.allInclusive"].ToString(),
                        };
                        fetchedHotels.Add(h);
                        Console.WriteLine(h.name);
                    }

                    return fetchedHotels;
                });
                return hotels;
            }
        }

        public void CreateReservation(string option, DateTime checkIn, DateTime checkOut, int guests, string hotel, string email)
        {
            using (var session = driver.Session())
            {
                int roomNumber = session.ReadTransaction(tx =>
                {
                    var rooms = tx.Run("" +
                        "MATCH (h:Hotel)-[:OFFERS]->(r:Room), (z:Reservation) " +
                        "WHERE h.name = $hotel " +
                        "AND r.beds >= $guests " +
                        "AND (" +
                        "(NOT (r)-[:TAKES]->()) " +
                        "OR " +
                        "((r)-[:TAKE]->(z) AND (z.checkIn > date($checkOut) OR z.checkOut < date($checkIn)))" +
                        ") " +
                        "RETURN distinct r.roomNumber",
                        new
                        {
                            hotel,
                            checkIn,
                            checkOut,
                            guests
                        });
                    return Int32.Parse(rooms.First()["r.roomNumber"].ToString());
                });

                var result = session.WriteTransaction(tx =>
                {
                    var query_result = tx.Run("" +
                        "MATCH (c:Client {email: $email}), " +
                        "(r:Room {roomNumber: $roomNumber})--(h:Hotel {name: $hotel}) " +
                        "CREATE (c)-[:RESERVED]->(res:Reservation {checkIn: date($checkIn), guests: $guests, " +
                        "checkOut: date($checkOut), option: $option})-[:TAKES]->(r)",
                        new
                        {
                            checkIn,
                            guests,
                            checkOut,
                            option,
                            email,
                            hotel,
                            roomNumber
                        });

                    var summary = query_result.Consume();
                    Console.WriteLine(summary);
                    return query_result;
                });
                return;
            }
        }

        public void CheckExpiredReservations(string today)
        {
            using (var session = driver.Session())
            {
                var result = session.WriteTransaction(tx =>
                {
                    var query_result = tx.Run("" +
                        "MATCH (res:Reservation)-[t:TAKES]->(room:Room) " +
                        "WHERE res.checkOut < date($today) " +
                        "DELETE t;",
                        new { today }
                       );

                    return query_result;
                });
                return;
            }
        }
    }
}
    