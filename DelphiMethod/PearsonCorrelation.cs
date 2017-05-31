using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DelphiMethod
{
    // Распределение Пирсона
    [Serializable]
    public struct PearsonCorrelation
    {
        // Таблица значений
        public double[,] P;
        // Список альф(степеней свободы)
        public List<double> Alphas;

        public PearsonCorrelation(string[] lines)
        {
            var height = lines.Length;
            var width = lines[1].Split(' ').Length;
            P = new double[height, width];

            var firstLine = lines[0].Split(' ');
            Alphas = firstLine.Select(Convert.ToDouble).ToList();

            for (var i = 0; i < lines.Length - 1; i++)
            {
                var line = lines[i + 1].Split(' ');
                for (var j = 0; j < line.Length; j++)
                {
                    P[i, j] = Convert.ToDouble(line[j]);
                }
            }
        }
    }
}
