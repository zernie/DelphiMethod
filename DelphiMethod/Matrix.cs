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
        // Показатель
        public Indicator Indicator;

        // Начальные коэффициенты компетентности экспертов K_i^0=1/m
        public List<double> InitialCompetenceCoefficient =>
            Enumerable.Repeat(1.0 / Width, Width)
            .ToList();

        public Matrix(double[,] data, Indicator indicator)
        {
            Data = data;
            Height = data.GetLength(0);
            Width = data.GetLength(1);
            Indicator = indicator;
        }

        public Matrix(int width, int height, Indicator indicator)
        {
            Width = width;
            Height = height;
            Indicator = indicator;
            // заполняем нулями
            Data = new double[Height, Width];
        }

        public double this[int row, int col] => Data[row, col];

        // Вычесть список из списка
        private List<double> Subtract(List<double> a, List<double> b)
        {
            var data = new List<double>(a.Count);
            for (var i = 0; i < a.Count; i++)
            {
                data.Add(a[i] - b[i]);
            }

            return data;
        }

        // Групповые оценки x_j^k=sum(q^k*K_i*x_i_j^k), i=1..m), j=1..n
        public List<double> GroupScores(List<double> competenceCoefficients)
        {
            var groupScores = new List<double>(Height);
            for (var i = 0; i < Height; i++)
            {
                var temp = new List<double>(Width);
                for (var j = 0; j < Width; j++)
                {
                    temp.Add(Indicator.Weight * competenceCoefficients[j] * Data[i, j]);
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
                var sum = temp.Sum();

                if (sum == 0.0)
                    throw new NotFiniteNumberException(
                        $"Данные в показателе '{Indicator.Title}' введены неправильно");


                list.Add(sum);
            }
            return list;
        }

        // Нормировочный коэффициент lambda=sum(sum(x_i * x_i_j, i=1..m), j=1..n)
        public double Lambda(List<double> averageScores)
        {
            var lambda = 0.0;
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    lambda += Data[i, j] * averageScores[i];
                }
            }

            return lambda;
        }

        public List<double> CompetenceCoefficients() =>
            CompetenceCoefficients(InitialCompetenceCoefficient);

        // Коэффициенты компетентности K_i=(1/lambda)*sum(x_j*x_i_j, j=1..n)
        public List<double> CompetenceCoefficients(List<double> competenceCoefficients, double e = 0.001)
        {
            var data = new List<double>(Width);
            var averageScores = AverageScores(competenceCoefficients);

            while(true)
            {
                data.Clear();

                for (var i = 0; i < Width - 1; i++)
                {
                    var sum = 0.0;

                    for (var j = 0; j < Height; j++)
                    {
                        sum += Data[j, i] * averageScores[j];
                    }
                    data.Add(1.0 / Lambda(averageScores) * sum);
                }

                // K_m = 1 - sum(K_i, i=1..m-1)
                data.Add(1.0 - data.Sum());

                // x_i^t-1
                var previousAverageScores = averageScores;
                // x_i^t
                averageScores = AverageScores(data);
                // Признак окончания итерационного процесса
                // max(|x_i^t - x_i^t-1|) < e
                var max = Subtract(averageScores, previousAverageScores).Max();
                if (Math.Abs(max) < e) break;
            }
            return data;
        }

        // Сумма рангов
        public List<double> RanksSum()
        {
            var data = new List<double>(Height); 
            for (var i = 0; i < Height; i++)
            {
                var temp = 0.0;
                for (var j = 0; j < Width; j++)
                {
                    temp += Data[i, j];
                }
                data.Add(temp);
            }

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
