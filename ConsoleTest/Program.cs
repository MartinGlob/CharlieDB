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

                var n = c.DeleteAll<Band>(con);

                var band = CreateTestData("Beatles", new [] {("1961-01-08", "Liverpool"),("1961-03-27", "Hamburg"),("1964-12-24","London")});
                var b1 =c.Insert(con,band);

                band = CreateTestData("Rolling Stones", new[] { ("1966-04-5", "Copenhagen"), ("1966-09-23", "London"), ("1973-09-28", "Munich") });
                var b2= c.Insert(con, band);

                var b = c.Get<Band>(con,b1);
                b = c.Get<Band>(con, b2);


            }
        }

        private static Band CreateTestData(string name, IEnumerable<(string when, string where)> concerts)
        {
            var band = new Band
            {
                Name = name,
                Concerts = concerts.Select(c => new Concert() { When = DateTime.Parse(c.when), Where = c.where}).ToList()
            };

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
