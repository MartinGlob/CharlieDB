# CharlieDB
CharlieDB is a 'Code first' no noncense ORM for .Net and any* SQL server.

## Features
- CodeFirst. Generates SQL scripts based on your classes
- Fluent configuration, so NO Attributes polluting your classes
- Imposes very few restrictions on your classes



CharlieDB was inspired by the simplicity of Dapper.

## Note:
Currently CharlieDb does NOT automatically handle any database changes or inconsistencies between your C# classes and the database.


## Querying properties in embedded classes

```<language>
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

	var cdb = new CharlieDb(connection);

	cdb.Register<Band>();

	// First time you might like this method

    var createDbScript = cdb.CreateDbScripts();  // Creates a SQL script the creates your database

	var dbOk = cdb.VerifyDb();

    var band = new Band()... create a band
    var id = cdb.Insert<Band>(connection, band);
    var data = cdb.Get<Band>(connection, id);

	


```

### Find all in Berlin

c.QueryMany<Student>(WhereClause:"Address.City = 'Berlin'")

Charlie stores the Student.Address property as JSON and will
automatically create a SQL query using JSON_VALUE, JSON_QUERY or what ever is needed.


