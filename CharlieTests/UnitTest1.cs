//using CharlieDb;

//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Data.SqlClient;

//namespace CharlieTests
//{
//    [TestClass]
//    public class UnitTest1
//    {
//        [TestMethod]
//        public void TestMethod1()
//        {
//            var c = new Charlie();
//            var rc = c.Add<TestClassA>();

//            Assert.AreEqual(typeof(TestClassA).FullName, rc.ClassType.FullName);

//            //Assert.AreEqual(ElementTypes.Int32,rc.Properties[0].ElementType);
//            //Assert.AreEqual(nameof(TestClassA.Tint), rc.Properties[0].Name);

//            //Assert.AreEqual(nameof(TestClassA.Tlong), rc.Properties[1].Name);
//            //Assert.AreEqual(ElementTypes.Int64, rc.Properties[1].ElementType);
//        }


//        [TestMethod]
//        public void ReadSingleTest()
//        {
//            var c = new Charlie();
//            var rc = c.Add<TestClassA>();

//            var tc = new TestClassA();

//            tc.Author = new Person() {Born = new DateTime(1964,12,8), Name = "Martin Glob"};
//            tc.Gender = Gender.Male;
//            tc.Tchar = 'X';
//            tc.Tdecimal = 127.12345M;
//            tc.Tstring = "This is soo nice ;-)";

//            //var x = c.Insert(tc);


//            var script = c.GenerateCreateSqlScript();

//            //using (var con = new SqlConnection("Data Source=MGLPC\\SQLEXPRESS;Initial Catalog=cdb;Integrated Security=True"))
//            //{
//            //    con.Open();

//            //    var o = c.ReadSingle<TestClassA>(con, 1);

//            //}


//        }

//    }

//    [SqlTableName("TableA")]
//    public class TestClassA
//    {
//        public int ClassId { get; set; }
//        // Specials
//        public bool Tbool { get; set; }
//        public Guid Tguid { get; set; }

//        // Numerics
//        public int Tint { get; set; }
//        public long Tlong { get; set; }
//        public Single Tsingle { get; set; }
//        public double Tdouble { get; set; }
//        public float Tfloat { get; set; }
//        public Decimal Tdecimal { get; set; }

//        // DateTime
//        public DateTime TDateTime { get; set; }
//        public DateTime? TDateTimeNullable { get; set; }

//        // Strings etc.
//        public char Tchar { get; set; }
//        public string Tstring { get; set; }

//        // Charlie attributes
//        [DbColumn(size:500)]
//        public string TstringLen { get; set; }

//        // enums
//        public Gender Gender { get; set; }
//        // Classes
//        public Person Author { get; set; }
//    }

//    public enum Gender { Female, Male, WhatEver}
//    public class Person
//    {
//        public string Name { get; set; }
//        public DateTime Born { get; set; }
//    }



//}
