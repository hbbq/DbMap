using Shouldly;
using System.Collections.Generic;
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
        
        [Theory]
        [InlineData(3, 3, false)]
        [InlineData(0.0, 0, false)]
        [InlineData(1.0, 1, false)]
        [InlineData(1.1, 1, false)]
        [InlineData(1.9, 2, false)]
        [InlineData("0", 0, false)]
        [InlineData("1", 1, false)]
        [InlineData(null, 0, true)]
        [InlineData("a", 0, true)]
        public static void MapToNullableInt(object source, int expectedResult, bool nullExpected)
        {
            var o = DbMap.Mapping.MapTo(source, typeof(int?), null) as int?;
            if (nullExpected)
            {
                o.HasValue.ShouldBeFalse();
            }
            else
            {
                o.Value.ShouldBe(expectedResult);
            }
        }

        //[Theory]
        //[InlineData(3, 3)]
        //[InlineData(0.0, 0)]
        //[InlineData(1.0, 1)]
        //[InlineData(1.1, 1.1)]
        //[InlineData(1.9, 1.9)]
        //[InlineData("0", 0)]
        //[InlineData("1", 1)]
        //[InlineData(null, 0)]
        //public static void MapToDecimal(object source, decimal expectedResult)
        //{
        //    var o = DbMap.Mapping.MapTo(source, typeof(decimal), 0m);
        //    o.ShouldBeOfType(typeof(decimal));
        //    ((decimal)o).ShouldBe(expectedResult);
        //}

        //[Theory]
        //[InlineData(0.0, false)]
        //[InlineData(1.0, true)]
        //[InlineData(1.1, true)]
        //[InlineData("0", false)]
        //[InlineData("1", true)]
        //[InlineData("false", false)]
        //[InlineData("true", true)]
        //[InlineData(null, false)]
        //public static void MapToBool(object source, bool expectedResult)
        //{
        //    var o = DbMap.Mapping.MapTo(source, typeof(bool), false);
        //    o.ShouldBeOfType(typeof(bool));
        //    ((bool)o).ShouldBe(expectedResult);
        //}

        private class TestClass<T>
        {
            public T A { get; set; }
            public T B { get; set; }
            public T C { get; set; }
            public T D { get; set; }
            public T E { get; set; }
        }

        private static Dictionary<string, object> GetTestDictionary() =>
            new Dictionary<string, object>
            {
                {"a", "1"},
                {"b", 1},
                {"c", 1.123m},
                {"d", 1.123 },
                {"e", null},
            };

        [Fact]
        public static void CreateObjectTStrings()
        {
            var o = DbMap.Mapping.CreateObject<TestClass<string>>(GetTestDictionary());
            o.ShouldNotBeNull();
            o.A.ShouldBe("1");
            o.B.ShouldBe("1");
            o.C.ShouldContain("123");
            o.D.ShouldContain("123");
            o.E.ShouldBeNull();
        }

        [Fact]
        public static void CreateObjectTInts()
        {
            var o = DbMap.Mapping.CreateObject<TestClass<int>>(GetTestDictionary());
            o.ShouldNotBeNull();
            o.A.ShouldBe(1);
            o.B.ShouldBe(1);
            o.C.ShouldBe(1);
            o.D.ShouldBe(1);
            o.E.ShouldBe(default(int));
        }

        [Fact]
        public static void CreateObjectTDecimals()
        {
            var o = DbMap.Mapping.CreateObject<TestClass<decimal>>(GetTestDictionary());
            o.ShouldNotBeNull();
            o.A.ShouldBe(1);
            o.B.ShouldBe(1);
            o.C.ShouldBe(1.123m);
            o.D.ShouldBe(1.123m);
            o.E.ShouldBe(default(decimal));
        }

    }
    
}
