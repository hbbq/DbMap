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

            }

        }
        
    }
    
}
