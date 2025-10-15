namespace SigTec.Lib.NetStd20.Data
{
  using System;
  using System.Collections.Generic;
  using System.Data;

  public static class DbCommandExtensions
  {
    public static IDbDataParameter SetParameter(this IDbCommand command, string parameterName, object? value, ParameterDirection direction = ParameterDirection.Input, DbType? dbType = default)
    {
      if (command.Parameters.Contains(parameterName))
      {
        command.Parameters.Remove(parameterName);
      }
      var parameter = command.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.Direction = direction;
      if (dbType != null)
      {
        parameter.DbType = dbType.Value;
      }
      parameter.Value = value ?? DBNull.Value;
      command.Parameters.Add(parameter);
      return parameter;
    }

    public static void SetParameters<T>(this IDbCommand command, IEnumerable<KeyValuePair<string, T>> parameters, bool bindEvenIfNotFoundInCommandText = false, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
    {
      foreach (var kvp in parameters)
      {
        var bindName = $"{command.Connection.GetParameterPrefix()}{kvp.Key}";
        var index = command.CommandText.IndexOf(bindName, stringComparison);
        if (index >= 0)
        {
          // get bindName in the exact casing from the CommandText
          bindName = command.CommandText.Substring(index, bindName.Length);
        }
        if (index >= 0 || bindEvenIfNotFoundInCommandText)
        {
          command.SetParameter(bindName.Substring(1), kvp.Value);
        }
      }
    }

    public static IEnumerable<IDataRecord> ExecuteEnumberable(this IDbCommand command)
    {
      using (var reader = command.ExecuteReader())
      {
        yield return reader;
      }
    }

    public static DataTable ExecuteQuery(this IDbCommand command, string? tableName = null)
    {
      var table = string.IsNullOrEmpty(tableName) ? new DataTable() : new DataTable(tableName);
      using (var reader = command.ExecuteReader())
      {
        table.Load(reader);
      }
      return table;
    }
  }
}
