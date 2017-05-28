using System;
using System.Collections.Generic;
using System.Linq;

namespace DelphiMethod
{
    // Матрица рангов
    [Serializable]
    public class Matrix
    {
        public double[,] X;
        // Кол-во экспертов (ширина)
        public int M;
        // Кол-во альтернатив (высота)
        public int N;
        // Показатель
        public Indicator Indicator;
        // Точность вычисления коэффициентов компетентности
        public const double E = 0.001;

        public Matrix(double[,] x, Indicator indicator)
        {
            X = x;
            N = x.GetLength(0);
            M = x.GetLength(1);
            Indicator = indicator;
        }

        public Matrix(int m, int n, Indicator indicator)
        {
            M = m;
            N = n;
            Indicator = indicator;
            // заполняем нулями
            X = new double[n, m];
        }

        public double this[int row, int col] => X[row, col];

        // Групповые оценки x_j^k = Σ(q^k * K_i * x_i_j^k), i=1..m), j=1..n
        public List<double> GroupScores(List<double> competenceCoefficients)
        {
            var groupScores = new List<double>(N);
            for (var i = 0; i < N; i++)
            {
                var sum = 0.0;
                for (var j = 0; j < M; j++)
                {
                    sum += Indicator.Weight * competenceCoefficients[j] * X[i, j];
                }
                groupScores.Add(sum);
            }

            return groupScores;
        }

        // Средние оценки объектов x_i = Σ(K_i * x_i_j, i = 1..m), j = 1..n
        public List<double> AverageScores(List<double> competenceCoefficients)
        {
            var list = new List<double>(N);
            for (var i = 0; i < N; i++)
            {
                var sum = 0.0;
                for (var j = 0; j < M; j++)
                {
                    sum += X[i, j] * competenceCoefficients[j];
                }

                if (sum == 0.0) throw new ArithmeticException($"Данные в показателе '{Indicator.Title}' введены неправильно");

                list.Add(sum);
            }
            return list;
        }

        // Нормировочный коэффициент lambda = Σ(Σ(x_i * x_i_j, i = 1..m), j = 1..n)
        public double Lambda(List<double> averageScores)
        {
            var lambda = 0.0;
            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < M; j++)
                {
                    lambda += X[i, j] * averageScores[i];
                }
            }

            return lambda;
        }

        public List<double> CompetenceCoefficients() => CompetenceCoefficients(InitialCompetenceCoefficient());

        // Коэффициенты компетентности K_i = (1 / lambda) * Σ(x_j * x_i_j, j = 1..n)
        public List<double> CompetenceCoefficients(List<double> competenceCoefficients)
        {
            var data = new List<double>(M);
            var averageScores = AverageScores(competenceCoefficients);

            while (true)
            {
                data.Clear();

                for (var i = 0; i < M - 1; i++)
                {
                    var sum = 0.0;

                    for (var j = 0; j < N; j++)
                    {
                        sum += X[j, i] * averageScores[j];
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
                if (Math.Abs(max) < E) break;
            }
            return data;
        }

        // Начальные коэффициенты компетентности экспертов K_i^0 = 1 / M
        public List<double> InitialCompetenceCoefficient()
        {
            var coefficients = new List<double>(M);
            for (var i = 0; i < M; i++)
            {
                coefficients.Add(1.0 / M);
            }

            return coefficients;
        }

        // Вычесть вектор из вектора
        private List<double> Subtract(List<double> a, List<double> b)
        {
            var data = new List<double>(a.Count);
            for (var i = 0; i < a.Count; i++)
            {
                data.Add(a[i] - b[i]);
            }

            return data;
        }


        // Вычиcление согласованности экспертов

        // Оценка математического ожидания r = 1 / 2 * m(n + 1)
        public double R() => 0.5 * M * (N + 1);

        // Сумма показателей рангов ΣT_i, i=1..H_i. T_i=Σ(h_k^3 - h_k, k = 1..H_i)
        public double T()
        {
            var T = 0.0;
            for (var i = 0; i < M; i++)
            {
                var temp = new List<double>(N);
                for (var j = 0; j < N; j++)
                {
                    temp.Add(X[j, i]);
                }

                var H = temp
                    .GroupBy(x => x) // группируем массив по одинаковым значениям
                    .Where(g => g.Count() > 1) // отсеиваем группы, где меньше 1 элемента
                    .Select(g => g.Count())   // берем количество элементов в группе
                    .ToList();

                var Ti = H.Sum(hk => (int) Math.Pow(hk, 3) - hk);

                T += Ti;
            }
            return T;
        }

        // S = Σ((Σ(r_ij - r, i = 1..m)) ^ 2, j = 1..n)
        public double S()
        {
            var s = 0.0;
            var r = R();

            for (var i = 0; i < N; i++)
            {
                var sum = 0.0;
                for (var j = 0; j < M; j++)
                {
                    sum += X[i, j];
                }
                s += Math.Pow(sum - r, 2);
            }
            return s;
        }

        // Коэффициент конкордации кенделла
        // W = 12S / (m^2 (n^3 - n) - m * Σ(T_i, i=1..m))
        public double W()
        {
            var t = T();

            return 12 * S() /
                (Math.Pow(M, 2) * (Math.Pow(N, 3) - N) - M * t);
        }

        // Заполнить матрицу случайными значениями
        public void FillWithRandomValues()
        {
            var rand = new Random();
            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < M; j++)
                {
                    X[i, j] = Math.Round(rand.NextDouble() * 10, 1);
                }
            }
        }
    }
}
