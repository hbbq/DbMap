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
            try
            {
                using (var cn = new SQLiteConnection($@"Data source={name}"))
                {
                    cn.Open();
                    cn.ExecuteScalar("select count(*) from a").ToString().ShouldBe("4");
                }
            }
            finally
            {
                CleanUp(name);
            }
        }

        [Fact]
        public static void ExecuteScalarTInt()
        {
            var name = CreateDb();
            try
            {
                using (var cn = new SQLiteConnection($@"Data source={name}"))
                {
                    cn.Open();
                    cn.ExecuteScalar<int>("select count(*) from a").ShouldBe(4);
                }
            }
            finally
            {
                CleanUp(name);
            }
        }

        [Fact]
        public static void ExecuteScalarTString()
        {
            var name = CreateDb();
            try
            {
                using (var cn = new SQLiteConnection($@"Data source={name}"))
                {
                    cn.Open();
                    cn.ExecuteScalar<string>("select count(*) from a").ShouldBe("4");
                }
            }
            finally
            {
                CleanUp(name);
            }
        }

        [Fact]
        public void ExecuteTTwoTypesIntString()
        {
            var name = CreateDb();
            try
            {
                using (var cn = new SQLiteConnection($@"Data source={name}"))
                {
                    cn.Open();
                    var list = cn.Execute<TwoTypes<int, string>>("select a as item1, b as item2 from a order by a");
                    list.ShouldNotBeNull();
                    list.Count.ShouldBe(4);
                    list[0].Item1.ShouldBe(1);
                    list[1].Item1.ShouldBe(2);
                    list[2].Item1.ShouldBe(3);
                    list[3].Item1.ShouldBe(4);
                    list[0].Item2.ShouldBe("a");
                    list[1].Item2.ShouldBe("b");
                    list[2].Item2.ShouldBe("c");
                    list[3].Item2.ShouldBe("d");
                }
            }
            finally
            {
                CleanUp(name);
            }
        }

        [Fact]
        public void ExecuteTTwoTypesStringChar()
        {
            var name = CreateDb();
            try
            {
                using (var cn = new SQLiteConnection($@"Data source={name}"))
                {
                    cn.Open();
                    var list = cn.Execute<TwoTypes<string, char>>("select a as item1, b as item2 from a order by a");
                    list.ShouldNotBeNull();
                    list.Count.ShouldBe(4);
                    list[0].Item1.ShouldBe("1");
                    list[1].Item1.ShouldBe("2");
                    list[2].Item1.ShouldBe("3");
                    list[3].Item1.ShouldBe("4");
                    list[0].Item2.ShouldBe('a');
                    list[1].Item2.ShouldBe('b');
                    list[2].Item2.ShouldBe('c');
                    list[3].Item2.ShouldBe('d');
                }
            }
            finally
            {
                CleanUp(name);
            }
        }

        private class TwoTypes<T1, T2>
        {
            public T1 Item1 { get; set; }
            public T2 Item2 { get; set; }
        }

    }

}
