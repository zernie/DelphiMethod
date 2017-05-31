using System;
using System.Collections.Generic;
using System.Linq;

namespace DelphiMethod
{
    // Список матриц рангов
    [Serializable]
    public class MatrixList
    {
        // Конфигурация
        public Config Configuration;

        // Матрицы рангов
        public List<Matrix> Matrices;

        public MatrixList(Config configuration)
        {
            Configuration = configuration;

            Matrices = new List<Matrix>(configuration.IndicatorsCount);

            Matrices = configuration.Indicators.Select(x =>
                new Matrix(configuration.ExpertsCount, configuration.AlternativesCount, x)
            ).ToList();
        }

        public Matrix this[int index]
        {
            get => Matrices[index];
            set => Matrices[index] = value;
        }

        // Матрица групповых оценок альтернатив по показателям
        public double[,] GroupScores()
        {
            var groupScores = new double[Configuration.AlternativesCount, Configuration.IndicatorsCount];

            for (var i = 0; i < Configuration.AlternativesCount; i++)
            {
                for (var j = 0; j < Configuration.IndicatorsCount; j++)
                {
                    var matrix = Matrices[j];
                    var data = matrix.GroupScores(matrix.CompetenceCoefficients());
                    groupScores[i, j] = data[i];
                }
            }

            return groupScores;
        }

        // Суммы групповых оценок по показателям
        public List<double> GroupScoresSums(double[,] groupScores)
        {
            var sums = new List<double>(Configuration.AlternativesCount);

            for (var i = 0; i < Configuration.AlternativesCount; i++)
            {
                var sum = 0.0;
                for (var j = 0; j < Configuration.IndicatorsCount; j++)
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
                var s = new List<int> {indexes[i] + 1};

                for (var j = i + 1; j < list.Length; j++)
                {
                    if (list[i] == list[j])
                    {
                        s.Add(indexes[j]);
                        i++;
                    }
                }

                var strings = s.Select(x => $"x{x}").ToArray();
                ranks.Add($"{k++}. {string.Join(", ", strings)}");
            }

            return ranks;
        }

        // Индексы показателей, в которых достигнута согласованность мнений
        public List<int> DisabledRanks(int alphaIndex)
        {
            var disabledRanks = new List<int>();
            for (var i = 0; i < Matrices.Count; i++)
            {
                var rank = Matrices[i];
                if (rank.IsConsensusReached(Configuration.PearsonCorrelationTable, alphaIndex))
                {
                    disabledRanks.Add(i);
                }
            }
            return disabledRanks;
        }

        // Согласованы ли мнения во всех показателях?
        public bool IsAnalysisDone(int alphaIndex) => 
            Matrices.All(rank => rank.IsConsensusReached(Configuration.PearsonCorrelationTable, alphaIndex));
    }
}