using System;
using System.Collections.Generic;
using System.Linq;

namespace DelphiMethod
{
    // Матрица рангов
    [Serializable]
    public class Matrix
    {
        // матрица рангов
        public double[,] Data;
        // ширина (кол-во экспертов)
        public int Width;
        // высота (кол-во альтернатив)
        public int Height;
        // вес коэфф. показателя, q_k
        public double WeightIndicator;

        // Начальные коэффициенты компетентности экспертов K_i^0=1/m
        public List<double> InitialCompetenceCoefficient =>
            Enumerable.Repeat(1.0 / Width, Width)
            .ToList();

        public Matrix(double[,] data, double weightIndicator)
        {
            Data = data;
            Height = data.GetLength(0);
            Width = data.GetLength(1);
            WeightIndicator = weightIndicator;
        }

        public Matrix(int width, int height, double weightIndicator)
        {
            Width = width;
            Height = height;
            WeightIndicator = weightIndicator;
            // заполняем нулями
            Data = new double[Height, Width];
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
//
//        private double NewMethod(List<double> a, List<double> b)
//        {
//            return Math.Abs(Subtract(a, b).Max());
//        }

        // Групповые оценки x_j^k=sum(q^k*K_i*x_i_j^k), i=1..m), j=1..n
        public List<double> GroupScores(List<double> competenceCoefficients)
        {
            var groupScores = new List<double>(Height);
            for (var i = 0; i < Height; i++)
            {
                var temp = new List<double>(Width);
                for (var j = 0; j < Width; j++)
                {
                    temp.Add(WeightIndicator * competenceCoefficients[j] * Data[i, j]);
                }
                groupScores.Add(temp.Sum());
            }

            return groupScores;
        }

        // Средние оценки объектов x_i=sum(K_i * x_i_j, i=1..m), j=1..n
        public List<double> AverageScores(List<double> competenceCoefficients)
        {
            var list = new List<double>(Height);
            for (var i = 0; i < Height; i++)
            {
                var temp = new List<double>(Width);
                for (var j = 0; j < Width; j++)
                {
                    temp.Add(Data[i, j] * competenceCoefficients[j]);
                }
                list.Add(temp.Sum());
            }
            return list;
        }

        // Нормировочный коэффициент lambda=sum(sum(x_i * x_i_j, i=1..m), j=1..n)
        public double Lambda(List<double> competenceCoefficients)
        {
            var sum = 0.0;
            for (var i = 0; i < Height; i++)
            {
                var temp = new List<double>(Width);
                for (var j = 0; j < Width; j++)
                {
                    temp.Add(Data[i, j] * AverageScores(competenceCoefficients)[i]);
                }
                sum += temp.Sum();
            }

            return sum;
        }

        public List<double> CompetenceCoefficients() =>
            CompetenceCoefficients(InitialCompetenceCoefficient);

        // Коэффициенты компетентности K_i=(1/lambda)*sum(x_j*x_i_j, j=1..n)
        public List<double> CompetenceCoefficients(List<double> competenceCoefficients)
        {
            var data = new List<double>(Width);
            var averageScores = AverageScores(competenceCoefficients);

            for (var i = 0; i < Width - 1; i++)
            {
                var temp = new List<double>(Height);

                for (var j = 0; j < Height; j++)
                {
                    temp.Add(Data[j, i] * averageScores[j]);
                }
                data.Add(1.0 / Lambda(competenceCoefficients) * temp.Sum());
            }

            // K_m = 1 - sum(K_i, i=1..m-1)
            data.Add(1.0 - data.Sum());

            return data;
        }

        // Заполнить матрицу случайными значениями
        public void FillWithRandomValues()
        {
            var rand = new Random();
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    Data[i, j] = Math.Round(rand.NextDouble() * 10, 1);
                }
            }
        }
    }
}
