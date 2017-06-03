using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DelphiMethod
{
    // Таблица критических значений корреляции Пирсона
    [Serializable]
    public struct PearsonCorrelation
    {
        // Таблица значений
        public double[,] P;
        // Список альф(степеней свободы)
        public List<double> Alphas;
        // количество строк
        public int Length;

        public PearsonCorrelation(string[] lines)
        {
            var height = lines.Length;
            var width = lines[1].Split(' ').Length;
            P = new double[height, width];
            Length = lines.Length - 1;

            var firstLine = lines[0].Split(' ');
            Alphas = firstLine.Select(Convert.ToDouble).ToList();

            for (var i = 0; i < Length; i++)
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
