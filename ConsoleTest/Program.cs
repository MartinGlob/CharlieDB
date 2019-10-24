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
        static void Main(string[] args)
        {
            var c = new Charlie();

            c.Register<Asset>()
                .IdProperty(nameof(Asset.Id))
                .Ignore(nameof(Asset.Name));

            c.Register<Identifier>()
                .IdProperty(nameof(Identifier.Id));

            var sql = c.GenerateCreateSqlScript();


            using (var con = new SqlConnection("Data Source=MGLPC\\SQLEXPRESS;Initial Catalog=cdb;Integrated Security=True"))
            {

                con.Open();

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

            Console.ReadLine();
            return;
        }
    }

   
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Identifier> Identifiers { get; set; }
    }

    public enum IdentifierSource
    {
        ISIN,
        Bloomberg,
        Moodys
    }

    public class Identifier 
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public IdentifierSource IdentifierSource { get; set; }
    }



}
