using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
    // Матрица рангов
    [Serializable]
    public class Matrix
    {
        public double[,] Data;
        public List<Alternative> Alternatives;
        public int Width;
        public int Height;

        public double WeightIndicator; // вес коэфф. показателя, q_k

        public List<double> InitialCompetenceCoefficient => 
            Enumerable
            .Repeat(1.0 / Width,  Width)
            .ToList();

        public Matrix(double[,] data, double weightIndicator)
        {
            Data = data;
            Height = data.GetLength(0);
            Width = data.GetLength(1);
            WeightIndicator = weightIndicator;
            Alternatives = new List<Alternative>(Height);

            _init(Alternatives);
        }

        public Matrix(int width, int height, double weightIndicator)
        {
            Width = width;
            Height = height;
            WeightIndicator = weightIndicator;
            Data = new double[Height, Width];
            Alternatives = new List<Alternative>(Height);

            _init(Alternatives);
        }

        private void _init(List<Alternative> alternatives)
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

        public List<double> GroupScores(List<double> competenceCoefficients)
        {
            var groupScores = new List<double>();
            for (int i = 0; i < Height; i++)
            {
                var temp= new List<double>();
                for (int j = 0; j < Width; j++)
                {
                   temp.Add( WeightIndicator * competenceCoefficients[i] * Data[i, j]);
                }
                groupScores.Add(temp.Sum());
            }

            return groupScores;
        }

        // Средние оценки объектов sum(K_i * x_i_j, i=1..m), j=1..n
        public List<double> AverageScores(List<double> competenceCoefficients) => 
            Alternatives
            .Select((x, i) => x.MultiplyBy(competenceCoefficients[i]))
            .ToList();

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
