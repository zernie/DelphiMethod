using System;
using System.Collections.Generic;
using System.Linq;

namespace DelphiMethod
{
//    using Alternative = List<decimal>;

    // Эксперт
    public class Expert
    {
        public List<decimal> Alternatives;

        public decimal Median
        {
            get
            {
                var temp = Alternatives.ToArray();
                Array.Sort(temp);
                var count = temp.Length;

                if (count % 2 != 0) return temp[count / 2];

                var a = temp[count / 2 - 1];
                var b = temp[count / 2];
                return (a + b) / 2m;
            }
        }

        public Expert(List<decimal> alternatives)
        {
            Alternatives = alternatives;
        }

        public Expert(int count)
        {
            Alternatives = new List<decimal>(new decimal[count]);
        }

        public override string ToString()
        {
            return string.Join(" ", Alternatives.Select(Convert.ToString).ToArray());
        }
    }

    // Матрица оценок
    public class Matrix
    {
        public List<Expert> Experts;
        public List<decimal> Medians => Experts.Select(x => x.Median).ToList();

        public Matrix()
        {
            Experts = new List<Expert>();
        }

        public Matrix(List<Expert> experts)
        {
            Experts = experts;
        }

        public Matrix(int rows, int cols)
        {
            Experts = new List<Expert>();
            for (var i = 0; i <= rows; i++)
            {
               Experts.Add(new Expert(cols));
            }
        }

        public decimal this[int x, int y] => Experts[x].Alternatives[y];

        public override string ToString()
        {
            return string.Join("\n", Experts.Select(Convert.ToString).ToArray());
        }
    }
}
