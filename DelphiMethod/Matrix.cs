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

        public int IndicatorNumber; // показатель k
        public double WeightIndicator => InitialData.WeightIndicators[IndicatorNumber]; // вес коэфф. показателя, q_k

        public List<double> InitialCompetenceCoefficient =>
            Experts
                .Select(x => 1.0 / InitialData.ExpertsCount)
                .ToList();

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

//        private List<double> Subtract(List<double> a, List<double> b)
//        {
//            var data = new List<double>(a.Count);
//            for (var i = 0; i < a.Count; i++)
//            {
//                data.Add(a[i] - b[i]);
//            }
//
//            return data;
//        }

//        private double NewMethod(List<double> a, List<double> b)
//        {
//            return Math.Abs(Subtract(a, b).Max());
//        }

        // Средние оценки объектов sum(K_i * x_i_j, i=1..m), j=1..n
        public List<double> AverageScores(List<double> competenceCoefficients)
        {
            return Alternatives
                .Select((x, i) => x.MultiplyBy(competenceCoefficients[i]))
                .ToList();
        }

        // Нормировочный коэффициент sum(sum(x_i * x_i_j, i=1..m), j=1..n)
        public double Lambda(List<double> competenceCoefficients) =>
            Alternatives
                .Select((x, i) => x.MultiplyBy(AverageScores(competenceCoefficients)[i]))
                .Sum();

        public List<double> CompetenceCoefficients() => 
            CompetenceCoefficients(InitialCompetenceCoefficient);

        public List<double> CompetenceCoefficients(List<double> competenceCoefficients)
        {
            var data = new List<double>(Width);

            for (var i = 0; i < Width - 1; i++)
            {
                var temp = new List<double>(Height);
                var coefficient = competenceCoefficients[i];

                for (var j = 0; j < Height; j++)
                {
                    temp.Add(Data[j, i] * Alternatives[j].MultiplyBy(coefficient));
                }
                data.Add(1.0 / Lambda(competenceCoefficients) * temp.Sum());
            }

            data.Add(1.0 - data.Sum());

            return data;
        }

        public override string ToString()
        {
            return string.Join("\n", Alternatives.Select(x => x.ToString()).ToArray());
        }
    }
}
