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
  }
}
