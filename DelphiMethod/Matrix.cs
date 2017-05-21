using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public int IndicatorNumber;  // показатель k
        public double WeightIndicator => InitialData.WeightIndicators[IndicatorNumber]; // вес коэфф. показателя, q_k

        // sum(q_k * K_i * x_i_j^k)
        public List<double> GroupEvaluations => 
            Alternatives
            .Select(x => x.GroupEvaluation(WeightIndicator, 0.1))
            .ToList();

        public double Lambda(double competenceCoffiecient) =>
            Alternatives
                .Select((x, i) => x.Lambda(x.Xjl(competenceCoffiecient), competenceCoffiecient))
                .Sum();

        public List<double> InitialCompetenceCoefficient => Experts.Select(x => 2.0 / InitialData.ExpertsCount).ToList();

        public Matrix(double[,] data, InitialData initialData, int indicatorNumber)
        {
            Data = data;
            InitialData = initialData;
            IndicatorNumber = indicatorNumber;

            Width = initialData.ExpertsCount;
            Height = initialData.AlternativesCount;

            Alternatives = new List<Alternative>(Height);
            Experts = new List<Expert>(Width);

            _init(Alternatives, Experts);
        }

        public Matrix(InitialData initialData, int indicatorNumber)
        {
            InitialData = initialData;
            IndicatorNumber = indicatorNumber;
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

        private List<double> calcCompetenceCofficient(List<double> competenceCoffiecients)
        {
            var data = new List<double>(Width);

            for (var i = 0; i < Width -1; i++)
            {
                var temp = new List<double>(Height);
                var coefficient = competenceCoffiecients[i];

                for (var j = 0; j < Height; j++)
                {
                    temp.Add(Data[j, i] * Alternatives[j].Xjl(coefficient));
                }
                data.Add(1.0 / Lambda(coefficient) * temp.Sum());
            }

            data.Add(1.0 - data.Sum());

            return data;
        }

        public List<double> CompetenceCoefficients(int times = 3)
        {
            return CompetenceCoefficients(InitialCompetenceCoefficient, times);
        }

        public List<double> CompetenceCoefficients(List<double> competenceCoefficients, int times)
        {
            for (var i = 0; i < times; i++)
            {
                competenceCoefficients = calcCompetenceCofficient(competenceCoefficients);
            }

            return competenceCoefficients;
        }

        public override string ToString()
        {
            return string.Join("\n", Alternatives.Select(x => x.ToString()).ToArray());
        }
    }
}
