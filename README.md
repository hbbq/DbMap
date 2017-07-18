# DbMap

A library with extensions for DbConnection to execute queries and optionally map the results to types

Possibility to execute a query directly from a DbConnection instance and casting the result to (almost) any type:
```C#
using (var cn = new SQLiteConnection(connectionString))
{
    cn.Open();
    var count = cn.ExecuteScalar<int>("select count(*) from MyTable");
}
```

Possibility to execute a query and return a list of objects of (almost) any type:
```C#
private class Person
{
   public int Id { get; set; }
   public string Firstname { get; set; }
   public string Lastname { get; set; }
}

private static void Main()
{
    var connectionString = "<ConnectionString>";
    using (var cn = new SQLiteConnection(connectionString))
    {
        cn.Open();
        var persons = cn.Execute<Person>("select id, firstname, lastname from Persons");
    }   
}
```

Possibility to send parameters to query as instance of an anonymous (or named) type:
```C#
using (var cn = new SQLiteConnection(connectionString))
{
    cn.Open();
    var id = cn.ExecuteScalar<int>(
        "select id from Persons where firstname = @firstname and lastname = @lastname",
        new
        {
            filename = "John",
            lastname = "Doe",
        });
}
```
