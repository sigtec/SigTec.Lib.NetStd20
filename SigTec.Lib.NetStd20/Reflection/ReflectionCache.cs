namespace SigTec.Lib.NetStd20.Reflection
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionCache<T>
    {
        /// <summary>
        /// All public, readable, non-indexed properties
        /// </summary>
        public static IReadOnlyDictionary<string, PropertyInfo> PublicProperties { get; } = typeof(T).GetProperties()
            .Where(p => p.CanRead && !p.GetIndexParameters().Any())
            .ToDictionary(p => p.Name);
            
        public static IReadOnlyDictionary<string, FieldInfo> PublicFields { get; } = typeof(T).GetFields().ToDictionary(f => f.Name);
    }
}
