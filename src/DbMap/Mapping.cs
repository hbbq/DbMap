using System;
using System.Linq;
using System.Collections.Generic;

namespace DbMap
{
    
    public static class Mapping
    {

        public static T CreateObject<T>(IEnumerable<KeyValuePair<string, object>> source) where T: new()
        {
            var obj = new T();
            FillObject<T>(source, ref obj);
            return obj;
        }

        public static void FillObject<T>(IEnumerable<KeyValuePair<string, object>> source, ref T target)
        {
            foreach(var pair in source)
            {
                foreach(var prop in typeof(T).GetProperties().Where(p => p.CanWrite && p.Name.ToLower() == pair.Key.ToLower()))
                {
                    var value = pair.Value;
                    if (value == DBNull.Value) value = null;
                    if (value != null)
                    {
                        var targetValue = MapTo(value, prop.PropertyType);
                        prop.SetValue(target, targetValue);
                    }
                }
            }
        }

        public static T MapTo<T>(object source)
        {

            if (source == DBNull.Value) source = null;

            if (source == null) return (T) GetDefault(typeof(T));

            if (source is T) return (T) source;

            try
            {
                return (T) ChangeType(source, typeof(T));
            }
            catch (FormatException)
            {
                throw new Exception($@"Can't cast source of type {source.GetType().Name} to {typeof(T).Name}");
            }

        }

        public static object MapTo(object source, Type targetType)
        {

            if (source == DBNull.Value) source = null;

            if (source == null) return GetDefault(targetType);

            if (source.GetType() == targetType || source.GetType().IsSubclassOf(targetType)) return source;

            try
            {
                return ChangeType(source, targetType);
            }
            catch (FormatException)
            {
                throw new Exception($@"Can't cast source of type {source.GetType().Name} to {targetType.Name}");
            }

        }

        private static object ChangeType(object source, Type targetType, bool fallbackToDefault = true)
        {
            try
            {
                return Convert.ChangeType(source, targetType);
            }
            catch
            {

                var innerType = Nullable.GetUnderlyingType(targetType);
                if (innerType != null)
                {
                    object v = null;
                    try
                    {
                        v = ChangeType(source, innerType, false);
                    }
                    catch
                    {
                        return GetDefault(targetType);
                    }
                    return Activator.CreateInstance(targetType, new[] { v });
                }
                                
                if (targetType == typeof(bool))
                {
                    var s = source as string;
                    if (s != null)
                    {
                        if (s == "0") return false;
                        if (s == "1") return true;
                    }
                }
                if(fallbackToDefault) return GetDefault(targetType);
                throw;
            }
        }

        private static object GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

    }
    
}
