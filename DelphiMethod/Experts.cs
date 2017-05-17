using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DelphiMethod
{
//    using Alternative = List<decimal>;

    // Эксперт
    public class Expert
    {
        public List<decimal> Alternatives;

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
