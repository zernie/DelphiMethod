using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DelphiMethod
{
    // Список матриц рангов
    [Serializable]
    public class MatrixList
    {
        // Настройки
        public Config Configuration;

        // Матрицы рангов
        public List<Matrix> Matrices;

        public MatrixList(Config configuration)
        {
            Configuration = configuration;

            Matrices = configuration.Indicators.Select(x =>
                new Matrix(configuration.M, configuration.N, x)
            ).ToList();
        }

        public MatrixList(Config configuration, List<double[,]> matrices)
        {
            Configuration = configuration;

            Matrices = new List<Matrix>();
            for (var i = 0; i < matrices.Count; i++)
            {
                Matrices.Add(new Matrix(matrices[i], configuration.Indicators[i]));
            }
        }

        public Matrix this[int index]
        {
            get => Matrices[index];
            set => Matrices[index] = value;
        }

        // Согласованы ли мнения во всех показателях?
        public bool IsAnalysisDone =>
            Matrices.All(rank => rank.IsConsensusReached(Configuration.PearsonCorrelationTable, Configuration.AlphaIndex));

        // xjk = Σ(q^k * Ki * xij^k), i=1..m), j=1..n
        // Матрица групповых оценок альтернатив по показателям
        public double[,] GroupScores()
        {
            var groupScores = new double[Configuration.N, Configuration.L];

            for (var i = 0; i < Configuration.N; i++)
            {
                for (var j = 0; j < Configuration.L; j++)
                {
                    var matrix = Matrices[j];
                    var data = matrix.xjk(matrix.Ki());
                    groupScores[i, j] = data[i];
                }
            }

            return groupScores;
        }

        // Групповые оценки
        // xj = Σ(q^k * Ki * xij^k), i=1..m), j=1..n, k=1..l
        public List<double> xj(double[,] groupScores)
        {
            var sums = new List<double>(Configuration.N);

            for (var i = 0; i < Configuration.N; i++)
            {
                var sum = 0.0;
                for (var j = 0; j < Configuration.L; j++)
                {
                    sum += groupScores[i, j];
                }
                sums.Add(sum);
            }

            return sums;
        }

        // Ранжирование альтернатив
        public List<string> Ranks(List<double> groupScoresSums)
        {
            var sum = groupScoresSums.Sum();
            var list = groupScoresSums.Select(x => x / sum).ToArray();
            var indexes = list.Select((x, i) => i).ToArray();

            Array.Sort(list, indexes);
            Array.Reverse(list);
            Array.Reverse(indexes);

            var ranks = new List<string>(list.Length);

            var k = 1;
            for (var i = 0; i < list.Length; i++)
            {
                var s = new List<int> { indexes[i] };

                for (var j = i + 1; j < list.Length; j++)
                {
                    if (list[i] == list[j])
                    {
                        s.Add(indexes[j]);
                        i++;
                    }
                }

                var strings = s.Select(x => Configuration.Alternatives[x]).ToArray();
                ranks.Add($"{k++}. {string.Join(", ", strings)}");
            }

            return ranks;
        }

        // Индексы показателей, в которых достигнута согласованность мнений
        public List<int> ConsensusReachedMatrices()
        {
            var data = new List<int>();
            for (var i = 0; i < Matrices.Count; i++)
            {
                var rank = Matrices[i];
                if (rank.IsConsensusReached(Configuration.PearsonCorrelationTable, Configuration.AlphaIndex))
                {
                    data.Add(i);
                }
            }
            return data;
        }

        // Очистить матрицы, где НЕ достигнута согласованность
        public void ClearWhereConsensusIsNotReached(Indicator indicator)
        {
            var matrices = ConsensusReachedMatrices();
            for (var i = 0; i <= matrices.Count; i++)
            {
                if (!matrices.Contains(i))
                    Matrices[i] = new Matrix(Configuration.M, Configuration.N, indicator);
            }
        }

        // Скопировать список матриц рангов
        public static MatrixList Clone( MatrixList source)
        {
            if (ReferenceEquals(source, null))
            {
                return default(MatrixList);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (MatrixList)formatter.Deserialize(stream);
            }
        }
    }
}