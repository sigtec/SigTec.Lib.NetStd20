using System;
using System.Data;

namespace SigTec.Lib.NetStd20.Data
{
  public static class DbConnectionExtensions
  {
    public static char GetParameterPrefix(this IDbConnection connection)
    {
      if (connection.GetType().FullName.Contains("oracle", System.StringComparison.InvariantCultureIgnoreCase))
      {
        return ':';
      }
      return '@';
    }

    public static IDbCommand CreateCommand(this IDbConnection connection, FormattableString commandText, IDbTransaction transaction = null)
    {
      var command = connection.CreateCommand();
      command.Transaction = transaction;
      command.SetCommandTextWithParameters(commandText);
      return command;
    }
  }
}
