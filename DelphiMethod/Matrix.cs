using System;
using System.Collections.Generic;
using System.Linq;

namespace DelphiMethod
{
    // Матрица рангов
    [Serializable]
    public class Matrix
    {
        // Матрица рангов xij^k
        public double[,] x { get; set; }
        // Кол-во экспертов (ширина)
        public int m;
        // Кол-во альтернатив (высота)
        public int n;
        // Вес коэффициента показателя
        public double qk => Indicator.Weight;
        // Точность вычисления коэффициентов компетентности
        public const double e = 0.001;
        // Показатель
        public Indicator Indicator;

        public Matrix(double[,] x, Indicator indicator)
        {
            this.x = x;
            n = x.GetLength(0);
            m = x.GetLength(1);
            Indicator = indicator;
        }

        public Matrix(int m, int n, Indicator indicator)
        {
            this.m = m;
            this.n = n;
            Indicator = indicator;
            // заполняем нулями
            x = new double[n, m];
        }

        public double this[int row, int col] => x[row, col];

        // Групповые оценки k показателя
        // xj^k = Σ(q^k * Ki * xij^k), i=1..m), j=1..n
        public List<double> xjk(List<double> Ki)
        {
            var xjk = new List<double>(n);
            for (var i = 0; i < n; i++)
            {
                var sum = 0.0;
                for (var j = 0; j < m; j++)
                {
                    sum += qk * Ki[j] * x[i, j];
                }
                xjk.Add(sum);
            }

            return xjk;
        }

        // Средние оценки объектов
        // xjt = Σ(Ki * xij, i = 1..m), j = 1..n
        public List<double> xjt(List<double> Ki)
        {
            var list = new List<double>(n);
            for (var i = 0; i < n; i++)
            {
                var sum = 0.0;
                for (var j = 0; j < m; j++)
                {
                    sum += x[i, j] * Ki[j];
                }

                if (sum == 0.0) throw new ArithmeticException($"Данные в показателе '{Indicator.Title}' введены неправильно");

                list.Add(sum);
            }
            return list;
        }

        // Нормировочный коэффициент
        // λ = Σ(Σ(xjt * xij, i = 1..m), j = 1..n)
        public double Lambda(List<double> xjt)
        {
            var lambda = 0.0;
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < m; j++)
                {
                    lambda += x[i, j] * xjt[i];
                }
            }

            return lambda;
        }

        // Коэффициенты компетентности 
        // Ki = (1 / lambda) * Σ(xj * xij, j = 1..n)
        public List<double> Ki()
        {
            // Начальные коэффициенты компетентности экспертов Ki^0 = 1 / m
            var initialCoefficients = new List<double>(m);
            for (var i = 0; i < m; i++)
            {
                initialCoefficients.Add(1.0 / m);
            }

            var data = new List<double>(m);
            // xjt^t
            var averageScores = xjt(initialCoefficients);
            // xjt^t-1
            List<double> previousAverageScores;

            do
            {
                data.Clear();

                for (var i = 0; i < m - 1; i++)
                {
                    var sum = 0.0;

                    for (var j = 0; j < n; j++)
                    {
                        sum += x[j, i] * averageScores[j];
                    }
                    data.Add(1.0 / Lambda(averageScores) * sum);
                }

                // Km = 1 - sum(Ki, i=1..m-1)
                data.Add(1.0 - data.Sum());

                // xj^t-1
                previousAverageScores = averageScores;
                // xj^t
                averageScores = xjt(data);
                // Признак окончания итерационного процесса
                // max(|xj^t - xj^t-1|) < e
            } while (Math.Abs(Subtract(averageScores, previousAverageScores).Max()) >= e);
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
            var Ti = new List<double>(m);
            for (var i = 0; i < m; i++)
            {
                var temp = new List<double>(n);
                for (var j = 0; j < n; j++)
                {
                    temp.Add(x[j, i]);
                }

                var H = temp
                    .GroupBy(x => x) // группируем массив по одинаковым значениям
                    .Where(g => g.Count() > 1) // отсеиваем группы, где меньше 1 элемента
                    .Select(g => g.Count())   // берем количество элементов в группе
                    .ToList();

                Ti.Add(H.Sum(hk => (int)Math.Pow(hk, 3) - hk));
            }
            return Ti;
        }

        // Сумма квадратов отклонений 
        // S = Σ((Σxij - ΣΣxij / n, i = 1..m)^2), j = 1..n)
        public double S()
        {
            var sums = new List<double>(n);
            var s = 0.0;
            var sum = 0.0;

            for (var i = 0; i < n; i++)
            {
                var temp = 0.0;
                for (var j = 0; j < m; j++)
                {
                    temp += x[i, j];
                }
                sum += temp;
                sums.Add(temp);
            }

            for (var i = 0; i < n; i++)
            {
                s += Math.Pow(sums[i] - sum / n, 2);
            }

            return s;
        }

        // Коэффициент конкордации Кенделла
        // W = 12S / (m^2 (n^3 - n) - m * Σ(Ti, i=1..m))
        public double W() => 12 * S() /
                             (Math.Pow(m, 2) * (Math.Pow(n, 3) - n) - m * Ti().Sum());

        // Критерий согласования Пирсона 
        // m(n - 1) * W
        public double X2() => m * (n - 1) * W();

        // Согласованы ли мнения?
        public bool IsConsensusReached(PearsonCorrelation pearsonCorrelationTable, int alphaIndex)
        {
            var x2 = X2();
            var x2Alpha = pearsonCorrelationTable.P[n - 2, alphaIndex];

            return x2Alpha < x2;
        }

        // Заполнить матрицу случайными значениями
        public void FillWithRandomValues()
        {
            var rand = new Random();
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < m; j++)
                {
                    x[i, j] = Math.Round(rand.NextDouble() * 10, 1);
                }
            }
        }
    }
}
