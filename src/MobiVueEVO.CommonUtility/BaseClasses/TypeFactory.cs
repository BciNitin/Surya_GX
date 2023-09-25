using System;
using System.Configuration;

namespace MobiVUE.Utility
{
    public class TypeFactory
    {
        public static T GetObject<T>(string logOnContextType) where T : class
        {
            var contextType = ConfigurationManager.AppSettings[logOnContextType];
            var type = Type.GetType(contextType);
            if (type != null)
                return Activator.CreateInstance(type) as T;
            else
                throw new NotImplementedException(logOnContextType);
        }
    }
}