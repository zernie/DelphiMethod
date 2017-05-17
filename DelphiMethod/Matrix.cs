using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
    // Альтернатива
    public class Alternative
    {
        public List<decimal> Values;

        public decimal Average => Math.Round(Values.Sum() / Values.Count, 2);

        public decimal Median
        {
            get
            {
                var temp = Values.ToArray();
                Array.Sort(temp);
                var count = temp.Length;

                if (count % 2 != 0) return temp[count / 2];

                var a = temp[count / 2 - 1];
                var b = temp[count / 2];
                return (a + b) / 2m;
            }
        }

        public Alternative(List<decimal> values)
        {
            Values = values;
        }

        public Alternative(int count)
        {
            Values = new List<decimal>(new decimal[count]);
        }

        public override string ToString()
        {
            return string.Join(" ", Values.Select(Convert.ToString).ToArray());
        }
    }

    // Матрица оценок
    public class Matrix
    {
        public List<Alternative> Alternatives;
        public List<decimal> Medians => Alternatives.Select(x => x.Median).ToList();
        public List<decimal> Averages => Alternatives.Select(x => x.Average).ToList();

        public Matrix()
        {
            Alternatives = new List<Alternative>();
        }

        public Matrix(List<Alternative> alternatives)
        {
            Alternatives = alternatives;
        }

        public Matrix(int rows, int cols)
        {
            Alternatives = new List<Alternative>();
            for (var i = 0; i <= rows; i++)
            {
               Alternatives.Add(new Alternative(cols));
            }
        }

        public decimal this[int row, int col] => Alternatives[row].Values[col];

        public sealed override string ToString()
        {
            return string.Join("\n", Alternatives.Select(Convert.ToString).ToArray());
        }
    }
}
