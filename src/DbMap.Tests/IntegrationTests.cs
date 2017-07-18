using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace DbMap.Tests
{

    public class IntegrationTests
    {

        private static string CreateDb()
        {
            var name = $"db_{Guid.NewGuid().ToString()}.sqlite";
            SQLiteConnection.CreateFile(name);
            using (var cn = new SQLiteConnection($@"Data source={name}"))
            using (var cm = cn.CreateCommand())
            {
                cn.Open();
                cm.CommandText = "create table a (a int not null, b text not null)";
                cm.ExecuteNonQuery();
                cm.CommandText = "insert into a (a, b) values (1, 'a')";
                cm.ExecuteNonQuery();
                cm.CommandText = "insert into a (a, b) values (2, 'b')";
                cm.ExecuteNonQuery();
                cm.CommandText = "insert into a (a, b) values (3, 'c')";
                cm.ExecuteNonQuery();
                cm.CommandText = "insert into a (a, b) values (4, 'd')";
                cm.ExecuteNonQuery();
            }
            return name;
        }

        private static void CleanUp(string name)
        {
            System.IO.File.Delete(name);
        }

        [Fact]
        public static void ExecuteScalar()
        {
            var name = CreateDb();
            using (var cn = new SQLiteConnection($@"Data source={name}"))
            {
                cn.Open();
                cn.ExecuteScalar("select count(*) from a").ToString().ShouldBe("4");
            }
            CleanUp(name);
        }

    }

}
