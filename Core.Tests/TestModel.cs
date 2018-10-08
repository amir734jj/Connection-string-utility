using System;
using System.Collections.Generic;
using Core.Tests.Extensions;

namespace Core.Tests
{
    public class TestModel
    {
        public string Prop1 { get; set; }

        public int Prop2 { get; set; }
        
        public decimal Prop3 { get; set; }
        
        public long Prop4 { get; set; }
        
        public DateTime Prop5 { get; set; }
        
        public Guid Prop6 { get; set; }
    }

    internal sealed class TestModelEqualityComparer : IEqualityComparer<TestModel>
    {
        public bool Equals(TestModel x, TestModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            
            return string.Equals(x.Prop1, y.Prop1)
                   && x.Prop2 == y.Prop2
                   && x.Prop3 == y.Prop3
                   && x.Prop4 == y.Prop4
                   && x.Prop5.SimpleEquals(y.Prop5)
                   && x.Prop6.Equals(y.Prop6);
        }

        public int GetHashCode(TestModel obj) => throw new NotImplementedException();
        
        public static IEqualityComparer<TestModel> TestModelComparer { get; } = new TestModelEqualityComparer();
    }
}