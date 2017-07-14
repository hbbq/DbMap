using System;

namespace DbMap
{
    
    public static class Mapping
    {

        public static T MapTo<T>(object source)
        {

            if (source == DBNull.Value) source = null;

            if (source == null) return default(T);

            if (source is T) return (T) source;

            try
            {
                return (T) Convert.ChangeType(source, typeof(T));
            }
            catch (FormatException)
            {
                throw new Exception($@"Can't cast source of type {source.GetType().Name} to {typeof(T).Name}");
            }

        }

        public static object MapTo(object source, Type targetType, object defaultValue)
        {

            if (source == DBNull.Value) source = null;

            if (source == null) return defaultValue;

            if (source.GetType() == targetType || source.GetType().IsSubclassOf(targetType)) return source;

            try
            {
                return Convert.ChangeType(source, targetType);
            }
            catch (FormatException)
            {
                throw new Exception($@"Can't cast source of type {source.GetType().Name} to {targetType.Name}");
            }

        }
        
    }
    
}
