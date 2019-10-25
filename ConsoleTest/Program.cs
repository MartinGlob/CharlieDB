using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CharlieDb;
using Newtonsoft.Json;

namespace ConsoleTest
{

    class Program
    {
        private const string ConnectionString = "Data Source=MGLPC\\SQLEXPRESS;Initial Catalog=cdb;Integrated Security=True";

        static void Main(string[] args)
        {
            DoExampleA();

            Console.ReadLine();
            return;
        }

        private static void DoExampleA()
        {
            var c = new Charlie();

            c.Register<Band>()
                .IdProperty(nameof(Band.Id));

            var sql = c.GenerateCreateSqlScript();


            using (var con = new SqlConnection(ConnectionString))
            {

                con.Open();

                var band = CreateTestData();

                var id = c.Insert(con,band);

                var b = c.Get<Band>(con,id);

                //var pi = c.Get<Pilot>(con, 2);

                //var p = new Pilot
                //{
                //    Name = "Martin Glob",
                //    ShortName = "MAG",
                //    Born = new DateTime(1964,12,08),
                //    CertificateIssued = new DateTime(1992,02,01)
                //};
                //var x = new Flights {BlockTimeInMin = 89, FromAirport = "EKRK", ToAirport = "EKEB"};
                //p.Flights.Add(x);
                //p.Flights.Add(x);
                //p.Flights.Add(new Flights { BlockTimeInMin = 90, FromAirport = "EKEB", ToAirport = "EKYT" });

                //var json = JsonConvert.SerializeObject(p.Flights);

                //p.Flights = JsonConvert.DeserializeObject<List<Flights>>(json);

                //var id = c.Insert(con, p);


            }
        }

        private static Band CreateTestData()
        {
            var band = new Band
            {
                Name = "Beatles",
            };

            band.Concerts.Add(new Concert()
            {
                When = new DateTime(1970,1,1),
                Where = "Manchester",

            });

            band.Tags.Add("A","Value A");
            band.Tags.Add("B", "Value B");
            band.Tags.Add("C", "Value C");

            band.Concerts.Add(new Concert()
            {
                When = new DateTime(1970, 2, 1),
                Where = "Berlin"
            });

            return band;
        }
    }


    public class Band
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Concert> Concerts { get; set; }
        public Dictionary<string,string> Tags { get; set; }

        public Band()
        {
            Concerts = new List<Concert>();
            Tags = new Dictionary<string, string>();
        }
    }

    public class Concert
    {
        //public int Id { get; set; }
        public int BandId { get; set; }
        public DateTime? When { get; set; }
        public string Where { get; set; }
    }


}
