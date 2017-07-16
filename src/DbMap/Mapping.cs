﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace DbMap
{
    
    public static class Mapping
    {

        public static T CreateObject<T>(Dictionary<string, object> source) where T: new()
        {
            var obj = new T();
            FillObject<T>(source, ref obj);
            return obj;
        }

        public static void FillObject<T>(Dictionary<string, object> source, ref T target)
        {
            foreach(var key in source.Keys)
            {
                foreach(var prop in typeof(T).GetProperties().Where(p => p.CanWrite && p.Name.ToLower() == key.ToLower()))
                {
                    var value = source[key];
                    if (value == DBNull.Value) value = null;
                    if (value != null)
                    {
                        var targetValue = MapTo(value, prop.PropertyType, null);
                        prop.SetValue(target, targetValue);
                    }
                }
            }
        }

        public static T MapTo<T>(object source)
        {

            if (source == DBNull.Value) source = null;

            if (source == null) return default(T);

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

        public static object MapTo(object source, Type targetType, object defaultValue)
        {

            if (source == DBNull.Value) source = null;

            if (source == null) return defaultValue;

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

        private static object ChangeType(object source, Type targetType)
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
                        v = ChangeType(source, innerType);
                    }
                    catch
                    {
                        if (innerType == typeof(int)) return new int?();
                    }
                    if (innerType == typeof(int)) return new int?((int)v);
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
                throw;
            }
        }
        
    }
    
}