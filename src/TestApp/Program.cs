﻿using System;
using System.Data.SQLite;
using DbMap;

namespace TestApp
{
    
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Program
    {
        
        private static void Main()
        {

            //Some integration tests with SQLite

            const string filename = "test.sqlite";
            
            SQLiteConnection.CreateFile(filename);

            using (var cn = new SQLiteConnection($@"Data source={filename}"))
            {

                cn.Open();

                cn.ExecuteNonQuery("create table a (a int, b text)");

                cn.ExecuteNonQuery("insert into a (a, b) values (1, '1')");
                cn.ExecuteNonQuery("insert into a (a, b) values (2, '3')");

                using (var reader = cn.ExecuteReader("select a, b from a"))
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($@"{reader.GetValue(0)}, {reader.GetValue(1)}");
                    }
                }

                // ReSharper disable UnusedVariable
                var o = cn.ExecuteScalar("select a from a");
                var Int = cn.ExecuteScalar<int>("select count(*) from a");
                var String = cn.ExecuteScalar<string>("select count(a) from a");
                var int2 = cn.ExecuteScalar<int>("select b from a where a = @a", new { a = 1 });
                var string2 = cn.ExecuteScalar<string>("select b from a");

                var list = cn.Execute<TestClass>("select * from a");

                // ReSharper restore UnusedVariable

            }

            Console.ReadLine();

        }

        private class TestClass
        {
            // ReSharper disable UnusedMember.Local
            public int A { get; set; }
            public string B { get; set; }
            // ReSharper restore UnusedMember.Local
        }
        
    }
    
}
