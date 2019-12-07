using System;
using System.Collections.Generic;

namespace Core
{
    public static class Locator
    {
        private readonly static Dictionary<string, Func<object>>
            s_services = new Dictionary<string, Func<object>>();

        public static void Register<T>(Func<T> resolver, string role = null)
        {
            string key = role ?? (typeof (T)).AssemblyQualifiedName;
            if ( key == null ) throw new NullReferenceException(KEY_NULL_MSG);
            s_services[key] = () => resolver();
        }

        public static T Resolve<T>(string role = null)
        {
            string key = role ?? (typeof(T)).AssemblyQualifiedName;
            if ( key == null ) throw new NullReferenceException(KEY_NULL_MSG);
            return (T)s_services[key]();
        }

        public static void Reset()
        {
            s_services.Clear();
        }

        private static string KEY_NULL_MSG = "Key for type resolver cannot be null.";
    }
}
