using System;
using System.Collections.Generic;
using System.Linq;

namespace DelphiMethod
{
    public struct Expert
    {
        public List<decimal> Values;

        public Expert(List<decimal> values) => Values = values;
        public Expert(int count) => Values = new List<decimal>(new decimal[count]);

        public override string ToString() => string.Join(" ", Values.Select(Convert.ToString).ToArray());
    }
}