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

        // Групповые оценки
        // xj^k = Σ(q^k * Ki * xij^k), i=1..m), j=1..n
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

        // Средние оценки объектов
        // xj = Σ(Ki * xij, i = 1..m), j = 1..n
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

        // Нормировочный коэффициент
        // λ = Σ(Σ(xi * xij, i = 1..m), j = 1..n)
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

        // Коэффициенты компетентности 
        // Ki = (1 / lambda) * Σ(xj * xij, j = 1..n)
        public List<double> Ki()
        {
            // Начальные коэффициенты компетентности экспертов Ki ^ 0 = 1 / m
            var initialCoefficients = new List<double>(M);
            for (var i = 0; i < M; i++)
            {
                initialCoefficients.Add(1.0 / M);
            }

            var data = new List<double>(M);
            // xi^t
            var averageScores = AverageScores(initialCoefficients);
            // xi^t-1
            List<double> previousAverageScores;

            do
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

                // Km = 1 - sum(Ki, i=1..m-1)
                data.Add(1.0 - data.Sum());

                // xj^t-1
                 previousAverageScores = averageScores;
                // xj^t
                averageScores = AverageScores(data);
                // Признак окончания итерационного процесса
                // max(|xj^t - xj^t-1|) < e
            } while (Math.Abs(Subtract(averageScores, previousAverageScores).Max()) >= E);
            return data;
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


        //                          Вычиcление согласованности экспертов

        // Показатель рангов 
        // Ti = Σ(hk^3 - hk, k = 1..Hi)
        public List<double> Ti()
        {
            var Ti = new List<double>(M);
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

                Ti.Add(H.Sum(hk => (int) Math.Pow(hk, 3) - hk));
            }
            return Ti;
        }

        // Сумма квадратов отклонений 
        // S = Σ((Σxij - ΣΣxij / n, i = 1..m)^2), j = 1..n)
        public double S()
        {
            var sums = new List<double>(N);
            var s = 0.0;
            var sum = 0.0;

            for (var i = 0; i < N; i++)
            {
                var temp = 0.0;
                for (var j = 0; j < M; j++)
                {
                    temp += X[i, j];
                }
                sum += temp;
                sums.Add(temp);
            }

            for (var i = 0; i < N; i++)
            {
                s += Math.Pow(sums[i] - sum / N, 2);
            }

            return s;
        }

        // Коэффициент конкордации Кенделла
        // W = 12S / (m^2 (n^3 - n) - m * Σ(Ti, i=1..m))
        public double W() => 12 * S() /
                             (Math.Pow(M, 2) * (Math.Pow(N, 3) - N) - M * Ti().Sum());

        // Критерий согласования Пирсона 
        // m(n - 1) * W
        public double X2() => M * (N - 1) * W();

        // Согласованы ли мнения?
        public bool IsConsensusReached(PearsonCorrelation pearsonCorrelationTable, int alphaIndex)
        {
            var x2 = X2(); 
            var x2Alpha = pearsonCorrelationTable.P[N - 2, alphaIndex];

            return x2Alpha < x2;
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
