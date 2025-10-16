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

    public static IEnumerable<IDataReader> ExecuteEnumberable(this IDbCommand command)
    {
      using (var reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          yield return reader;
        }
      }
    }

    public static IEnumerable<T0> ExecuteEnumberable<T0>(this IDbCommand command)
    {
      using (var reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          yield return reader.GetValue<T0>();
        }
      }
    }

    public static IEnumerable<(T0, T1)> ExecuteEnumberable<T0, T1>(this IDbCommand command)
    {
      using (var reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          yield return reader.GetValues<T0, T1>();
        }
      }
    }

    public static IEnumerable<(T0, T1, T2)> ExecuteEnumberable<T0, T1, T2>(this IDbCommand command)
    {
      using (var reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          yield return reader.GetValues<T0, T1, T2>();
        }
      }
    }

    public static IEnumerable<(T0, T1, T2, T3)> ExecuteEnumberable<T0, T1, T2, T3>(this IDbCommand command)
    {
      using (var reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          yield return reader.GetValues<T0, T1, T2, T3>();
        }
      }
    }

    public static IEnumerable<(T0, T1, T2, T3, T4)> ExecuteEnumberable<T0, T1, T2, T3, T4>(this IDbCommand command)
    {
      using (var reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          yield return reader.GetValues<T0, T1, T2, T3, T4>();
        }
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

    public static void SetCommandTextWithParameters(this IDbCommand command, FormattableString formattableString)
    {
      var i = 0;
      var binds = new object[formattableString.ArgumentCount];
      var parameterPrefix = command.Connection.GetParameterPrefix();
      foreach (var arg in formattableString.GetArguments())
      {
        var name = $"p{i}";
        binds[i++] = $"{parameterPrefix}{name}";
        command.SetParameter(name, arg);
      }
      command.CommandText = string.Format(formattableString.Format, binds);
    }
  }
}
