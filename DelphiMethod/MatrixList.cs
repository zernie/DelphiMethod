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
        
        // матрицы рангов
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
                        throw new ArithmeticException($"Данные в показателе '{indicator}' введены неправильно");
                    }

                    groupScores[i, j] = data[i];
                }
            }

            return groupScores;
        }

        public List<double> Sums()
        {
            var sums = new List<double>(Configuration.AlternativesCount);
            var groupScores = GroupScores();

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
    }
}
