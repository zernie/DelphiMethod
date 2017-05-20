using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
    // Матрица рангов
    public struct Matrix
    {
        public decimal[,] Data;
        public List<Alternative> Alternatives;
        public List<Expert> Experts;
        public InitialData InitialData;
        public int Width;
        public int Height;

        public int Indicator;  // показатель k
        public decimal WeightIndicator => InitialData.WeightIndicators[Indicator]; // вес коэфф. показателя, q_k

        // sum(q_k * K_i * x_i_j^k)
        public List<decimal> GroupEvaluations => Alternatives.Select(Selector).ToList();
        private decimal Selector(Alternative x) => x.GroupEvaluation(WeightIndicator);

        public Matrix(decimal[,] data, InitialData initialData, int indicator)
        {
            Data = data;
            InitialData = initialData;
            Indicator = indicator;

            Width = initialData.ExpertsCount;
            Height = initialData.AlternativesCount;

            Alternatives = new List<Alternative>(Height);
            Experts = new List<Expert>(Width);

            _init(Alternatives, Experts);
        }

        public Matrix(InitialData initialData, int indicator)
        {
            InitialData = initialData;
            Indicator = indicator;
            Width = initialData.ExpertsCount;
            Height = initialData.AlternativesCount;

            Data = new decimal[Height, Width];
            Alternatives = new List<Alternative>(Height);
            Experts = new List<Expert>(Width);

            _init(Alternatives, Experts);
        }

        private void _init(List<Alternative> alternatives, List<Expert> experts)
        {
            for (var i = 0; i < Height; i++)
            {
                var temp = new List<decimal>(Width);
                for (var j = 0; j < Width; j++)
                {
                    temp.Add(Data[i, j]);
                }
                alternatives.Add(new Alternative(temp));
            }

            for (var i = 0; i < Width; i++)
            {
                var temp = new List<decimal>(Height);
                for (var j = 0; j < Height; j++)
                {
                    temp.Add(Data[j, i]);
                }
                experts.Add(new Expert(temp));
            }
        }

        public decimal this[int row, int col] => Data[row, col];

        public override string ToString()
        {
            return string.Join("\n", Alternatives.Select(x => x.ToString()).ToArray());
        }
    }

    public struct Expert
    {
        public List<decimal> Values;

        public Expert(List<decimal> values) => Values = values;
        public Expert(int count) => Values = new List<decimal>(new decimal[count]);

        public override string ToString() => string.Join(" ", Values.Select(Convert.ToString).ToArray());
    }
}
