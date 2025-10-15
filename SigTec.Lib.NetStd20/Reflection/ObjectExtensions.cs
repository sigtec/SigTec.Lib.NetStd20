using System.Collections.Generic;
using System.Linq;

namespace SigTec.Lib.NetStd20.Reflection
{
    public static class ObjectExtensions
    {
        public static IReadOnlyDictionary<string, object> Reflect<T>(this T instance)
        =>  ReflectionCache<T>.PublicProperties.Values.ToDictionary(p => p.Name, p => p.GetValue(instance));
    }
}
