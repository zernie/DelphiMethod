using System;
using System.Collections.Generic;
using System.Linq;

namespace DelphiMethod
{
    [Serializable]
    public class MatrixList
    {
        // Конфигурация
        public Config Configuration;
        
        // Матрицы рангов
        public List<Matrix> Matrices = new List<Matrix>();
        //весовые коэффициенты показателей сравнения альтернатив; 

        public MatrixList(Config configuration)
        {
            Configuration = configuration;
        }

        public Matrix this[int index]
        {
            get => Matrices[index];
            set => Matrices[index] = value;
        }

        // Матрица групповых оценок на альтернативы
        public double[,] GroupScores()
        {
            var groupScores = new double[Configuration.AlternativesCount, Configuration.IndicatorsCount];

            for (var i = 0; i < Configuration.AlternativesCount; i++)
            {
                for (var j = 0; j < Configuration.IndicatorsCount; j++)
                {
                    var matrix = Matrices[j];
                    var data = matrix.GroupScores(matrix.CompetenceCoefficients());
                    if (double.IsNaN(data[i]))
                    {
                        var indicator = Configuration.WeightIndicators.Titles[j];
                        throw new NotFiniteNumberException($"Данные в показателе '{indicator}' введены неправильно");
                    }

                    groupScores[i, j] = data[i];
                }
            }

            return groupScores;
        }

        public List<double> GroupScoresSums(double[,] groupScores)
        {
            var sums = new List<double>(Configuration.AlternativesCount);

            for (var i = 0; i < Configuration.AlternativesCount; i++)
            {
                var temp = new List<double>(Configuration.IndicatorsCount);
                for (var j = 0; j < Configuration.IndicatorsCount; j++)
                {
                    temp.Add(groupScores[i, j]);
                }
                sums.Add(temp.Sum());
            }

            return sums;
        }

        public List<string> Ranks(List<double> groupScoresSums)
        {
            var sum = groupScoresSums.Sum();

            var list = groupScoresSums.Select(x =>  x / sum).ToArray();
            var indexes = list.Select((x, i) => i).ToArray();

            Array.Sort(list, indexes);
            Array.Reverse(list);
            Array.Reverse(indexes);
            var ranks = new List<string>();

            for (var i = 0; i < list.Length; i++)
            {
                var index = indexes[i];
                var item = Math.Round(groupScoresSums[index],3);
                ranks.Add($"{i+1}. x{index+1} = {item}");
            }

            return ranks;
        }
    }
}
