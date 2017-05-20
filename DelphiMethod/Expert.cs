using System;
using System.Collections.Generic;
using System.Linq;

namespace DelphiMethod
{
    public struct Expert
    {
        public List<double> Values;

        public Expert(List<double> values) => Values = values;
        public Expert(int count) => Values = new List<double>(new double[count]);

        public override string ToString() => string.Join(" ", Values.Select(Convert.ToString).ToArray());
    }
}