using System;
using System.Collections.Generic;
using System.Linq;

namespace DelphiMethod
{
    // Альтернатива
    [Serializable]
    public struct Alternative
    {
        public List<double> Values;

        public Alternative(List<double> values) => Values = values;

        public Alternative(int count) => Values = new List<double>(new double[count]);

        public double MultiplyBy(double value) => Values.Sum() * value;

        public double GroupEvaluation(double indicator, double competenceCoefficient)
        {
            return Values.Select(x => indicator * competenceCoefficient * x).Sum();
        }

        public override string ToString() => string.Join(" ", Values.Select(Convert.ToString).ToArray());
    }
}