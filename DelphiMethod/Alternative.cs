using System;
using System.Collections.Generic;
using System.Linq;

namespace DelphiMethod
{
    // Альтернатива
    public class Alternative
    {
        public List<decimal> Values;

        public Alternative(List<decimal> values) => Values = values;
        public Alternative(int count) => Values = new List<decimal>(new decimal[count]);

        public decimal GroupEvaluation(decimal indicator, decimal competenceCoefficient = 0.1M)
        {
            return Values.Select(x => indicator * competenceCoefficient * x).Sum();
        }

        public override string ToString() => string.Join(" ", Values.Select(Convert.ToString).ToArray());
    }
}