namespace SigTec.Lib.NetStd20.Tests.Data
{
  using System.Data;
  using Microsoft.Data.Sqlite;
  using SigTec.Lib.NetStd20.Data;
  using SigTec.Lib.NetStd20.Collections.Generic;

  [TestClass]
  public class DbCommandExtensionsTests
  {
    private readonly IDbConnection _connection = new SqliteConnection("Data Source=:memory:");

    [TestInitialize]
    public void Init()
    {
      _connection.Open();
      using (var cmd = _connection.CreateCommand())
      {
        cmd.CommandText = "CREATE TABLE Users(Id INTEGER PRIMARY KEY, Name TEXT)";
        cmd.ExecuteNonQuery();
        cmd.CommandText = "INSERT INTO Users(Id, Name) VALUES (1, 'Alice')";
        cmd.ExecuteNonQuery();
        cmd.CommandText = "INSERT INTO Users(Id, Name) VALUES (2, 'Bob')";
        cmd.ExecuteNonQuery();
        cmd.CommandText = "INSERT INTO Users(Id, Name) VALUES (3, 'Chris')";
        cmd.ExecuteNonQuery();
        cmd.CommandText = "INSERT INTO Users(Id, Name) VALUES (4, 'Dave')";
        cmd.ExecuteNonQuery();
        cmd.CommandText = "INSERT INTO Users(Id, Name) VALUES (5, 'Emma')";
        cmd.ExecuteNonQuery();
      }
    }

    [TestCleanup]
    public void Cleanup()
    {
      _connection.Close();
    }

    [TestMethod]
    public void ExecuteEnumerableValueSingleTest()
    {
      var id = 1;
      using (var cmd = _connection.CreateCommand($"SELECT Name FROM Users WHERE Id = {id}"))
      {
        var expected = "Alice";
        var actual = cmd.ExecuteEnumberable<string>().Single(); 
        Assert.AreEqual(expected, actual);
      }
    }

    [TestMethod]
    public void ExecuteEnumerableTupleTest()
    {
      var id = 3;
      using (var cmd = _connection.CreateCommand($"SELECT Id, Name FROM Users WHERE Id < {id} ORDER BY Id DESC"))
      {
        var actual = cmd.ExecuteEnumberable<int, string>().ToArray();
        Assert.AreEqual((2, "Bob"), actual[0]);
        Assert.AreEqual((1, "Alice"), actual[1]);
      }
    }

    [TestMethod]
    public void ExecuteEnumerableToDictionaryTest()
    {
      var id = 5;
      using (var cmd = _connection.CreateCommand($"SELECT Id, Name FROM Users WHERE Id = {id} ORDER BY Id DESC"))
      {
        var expected = "Emma's id is 5.";
        var actual = cmd.ExecuteEnumberable()
          .Select(r => r.ToDictionary())
          .Single()
          .ToFormattableString("{Name}'s id is {Id}.")
          .ToString();
        Assert.AreEqual(expected, actual);
      }
    }
  }
}