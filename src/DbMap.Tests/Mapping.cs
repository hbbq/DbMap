﻿using Shouldly;
using Xunit;

namespace DbMap.Tests
{
    
    public class Mapping
    {

        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "1")]
        [InlineData("a", "a")]
        [InlineData(null, null)]
        public static void MapToTString(object source, string expectedResult)
        {
            DbMap.Mapping.MapTo<string>(source).ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "1")]
        [InlineData("a", "a")]
        [InlineData(null, "")]
        public static void MapToString(object source, string expectedResult)
        {
            var o = DbMap.Mapping.MapTo(source, typeof(string), "");
            o.ShouldBeOfType(typeof(string));
            ((string)o).ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData(3, 3)]
        [InlineData(0.0, 0)]
        [InlineData(1.0, 1)]
        [InlineData(1.1, 1)]
        [InlineData(1.9, 2)]
        [InlineData("0", 0)]
        [InlineData("1", 1)]
        [InlineData(null, 0)]
        public static void MapToInt(object source, int expectedResult)
        {
            var o = DbMap.Mapping.MapTo(source, typeof(int), 0);
            o.ShouldBeOfType(typeof(int));
            ((int)o).ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData(3, 3)]
        [InlineData(0.0, 0)]
        [InlineData(1.0, 1)]
        [InlineData(1.1, 1.1)]
        [InlineData(1.9, 1.9)]
        [InlineData("0", 0)]
        [InlineData("1", 1)]
        [InlineData(null, 0)]
        public static void MapToDecimal(object source, decimal expectedResult)
        {
            var o = DbMap.Mapping.MapTo(source, typeof(decimal), 0m);
            o.ShouldBeOfType(typeof(decimal));
            ((decimal)o).ShouldBe(expectedResult);
        }
        
        [Theory]
        [InlineData(0.0, false)]
        [InlineData(1.0, true)]
        [InlineData(1.1, true)]
        [InlineData("0", false)]
        [InlineData("1", true)]
        [InlineData("false", false)]
        [InlineData("true", true)]
        [InlineData(null, false)]
        public static void MapToBool(object source, bool expectedResult)
        {
            var o = DbMap.Mapping.MapTo(source, typeof(bool), false);
            o.ShouldBeOfType(typeof(bool));
            ((bool)o).ShouldBe(expectedResult);
        }

    }
    
}
