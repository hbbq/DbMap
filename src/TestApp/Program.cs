using System;
using System.Data.SQLite;
using DbMap;

namespace TestApp
{
    
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Program
    {
        
        private static void Main()
        {

            const string filename = "test.sqlite";
            
            SQLiteConnection.CreateFile(filename);

            using (var cn = new SQLiteConnection($@"Data source={filename}"))
            {

                cn.Open();

                cn.ExecuteNonQuery("create table a (a int, b text)");

                cn.ExecuteNonQuery("insert into a (a, b) values (1, 'a')");

                using (var reader = cn.ExecuteReader("select a, b from a"))
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($@"{reader.GetValue(0)}, {reader.GetValue(1)}");
                    }
                }

            }

            Console.ReadLine();

        }
        
    }
    
}
