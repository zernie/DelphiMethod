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
        // Матрица рангов
        public double[,] Data;
        // Кол-во экспертов (ширина)
        public int M;
        // Кол-во альтернатив (высота)
        public int N;
        // Показатель
        public Indicator Indicator;

        // Начальные коэффициенты компетентности экспертов K_i^0=1/M
        public List<double> InitialCompetenceCoefficient
        {
            get
            {
                var coefficients = new List<double>(M);
                for (var i = 0; i < M; i++)
                {
                    coefficients.Add(1.0 / M);
                }

                return coefficients;
            }
        }

        public Matrix(double[,] data, Indicator indicator)
        {
            Data = data;
            N = data.GetLength(0);
            M = data.GetLength(1);
            Indicator = indicator;
        }

        public Matrix(int m, int n, Indicator indicator)
        {
            M = m;
            N = n;
            Indicator = indicator;
            // заполняем нулями
            Data = new double[n, m];
        }

        public double this[int row, int col] => Data[row, col];

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

        // Групповые оценки x_j^k=sum(q^k*K_i*x_i_j^k), i=1..M), j=1..N
        public List<double> GroupScores(List<double> competenceCoefficients)
        {
            var groupScores = new List<double>(N);
            for (var i = 0; i < N; i++)
            {
                var sum = 0.0;
                for (var j = 0; j < M; j++)
                {
                    sum += Indicator.Weight * competenceCoefficients[j] * Data[i, j];
                }
                groupScores.Add(sum);
            }

            return groupScores;
        }

        // Средние оценки объектов x_i=sum(K_i * x_i_j, i=1..M), j=1..N
        public List<double> AverageScores(List<double> competenceCoefficients)
        {
            var list = new List<double>(N);
            for (var i = 0; i < N; i++)
            {
                var sum = 0.0;
                for (var j = 0; j < M; j++)
                {
                    sum += Data[i, j] * competenceCoefficients[j];
                }

                if (sum == 0.0) throw new ArithmeticException($"Данные в показателе '{Indicator.Title}' введены неправильно");

                list.Add(sum);
            }
            return list;
        }

        // Нормировочный коэффициент lambda=sum(sum(x_i * x_i_j, i=1..m), j=1..N)
        public double Lambda(List<double> averageScores)
        {
            var lambda = 0.0;
            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < M; j++)
                {
                    lambda += Data[i, j] * averageScores[i];
                }
            }

            return lambda;
        }

        public List<double> CompetenceCoefficients() => CompetenceCoefficients(InitialCompetenceCoefficient);

        // Коэффициенты компетентности K_i=(1/lambda)*sum(x_j*x_i_j, j=1..n)
        public List<double> CompetenceCoefficients(List<double> competenceCoefficients, double e = 0.001)
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
            var data = new List<double>(N);
            for (var i = 0; i < N; i++)
            {
                var sum = 0.0;
                for (var j = 0; j < M; j++)
                {
                    sum += Data[i, j];
                }
                data.Add(sum);
            }

            return data;
        }

        public List<double> Ranks()
        {
            var data = new List<double>(M);
            for (var i = 0; i < M; i++)
            {
                var sum = 0.0;
                for (var j = 0; j < N; j++)
                {
                    sum += Data[j, i];
                }
                data.Add(sum);
            }

            return data;
        }

        public double R()
        {
            return 0.5 * M * (N + 1);
        }

        public double D()
        {
            var ranksSum = RanksSum();
            var r = R();

            var sum = 0.0;
            foreach (var t in ranksSum)
            {
                sum += Math.Pow(t - r, 2);
            }

            return 1.0 / (N - 1.0) * sum;
        }

        // (M^2 * (N^3 - N)) / (12 * (N - 1))
        public double DMax()
        {
            return (Math.Pow(M, 2) * (Math.Pow(N, 3) - N)) /
                   (12 * (N - 1));
        }

        public void T()
        {
            var H = Ranks().GroupBy(r => r).Select(g => g).ToList();
            MessageBox.Show(H.Count.ToString());

            /*for (var i = 0; i < H.Count; i++)
            {
                var h = H[i];
                var temp = new List<double>();

                for (var j = 0; j < M; j++)
                {
                    temp.Add(Data[i, j]);
                }
                MessageBox.Show(string.Join(" ", temp.Select(x => x.ToString()).ToArray()));

                MessageBox.Show($"!!! {temp.GroupBy(x => x).Count()}");
            }*/
        }

        public double W()
        {
            return D() / DMax();
        }

        // Заполнить матрицу случайными значениями
        public void FillWithRandomValues()
        {
            var rand = new Random();
            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < M; j++)
                {
                    Data[i, j] = Math.Round(rand.NextDouble() * 10, 1);
                }
            }
        }
    }
}
