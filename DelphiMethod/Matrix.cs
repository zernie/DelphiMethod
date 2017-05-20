using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
    // Матрица рангов
    public class Matrix
    {
        public double[,] Data;
        public List<Alternative> Alternatives;
        public List<Expert> Experts;
        public InitialData InitialData;
        public int Width;
        public int Height;

        public int Indicator;  // показатель k
        public double WeightIndicator => InitialData.WeightIndicators[Indicator]; // вес коэфф. показателя, q_k

        // sum(q_k * K_i * x_i_j^k)
        public List<double> GroupEvaluations => Alternatives.Select(x=> x.GroupEvaluation(WeightIndicator, 0.1)).ToList();

        public double InitialCompetenceCoefficient => 1.0 / InitialData.ExpertsCount;

        public Matrix(double[,] data, InitialData initialData, int indicator)
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

            Data = new double[Height, Width];
            Alternatives = new List<Alternative>(Height);
            Experts = new List<Expert>(Width);

            _init(Alternatives, Experts);
        }


        private void _init(List<Alternative> alternatives, List<Expert> experts)
        {
            for (var i = 0; i < Height; i++)
            {
                var temp = new List<double>(Width);
                for (var j = 0; j < Width; j++)
                {
                    temp.Add(Data[i, j]);
                }
                alternatives.Add(new Alternative(temp));
            }

            for (var i = 0; i < Width; i++)
            {
                var temp = new List<double>(Height);
                for (var j = 0; j < Height; j++)
                {
                    temp.Add(Data[j, i]);
                }
                experts.Add(new Expert(temp));
            }
        }

        public double this[int row, int col] => Data[row, col];

        public List<double> Xjl
        {
            get
            {
                var xjl = new List<double>(Height);
                for (var i = 0; i < Height; i++)
                {
                    xjl.Add(Alternatives[i].Values.Sum() * InitialCompetenceCoefficient);
                }

                return xjl;
            }
        }

        public double Lambda
        {
            get
            {
                var temp = new List<double>(Height);
                for (var i = 0; i < Height; i++)
                {
                    temp.Add(Alternatives[i].Values.Sum() * Xjl[i]);
                }

                return temp.Sum();
            }
        }

        //        public List<double> CompetenceCoefficients => 

        public override string ToString()
        {
            return string.Join("\n", Alternatives.Select(x => x.ToString()).ToArray());
        }
    }
}
