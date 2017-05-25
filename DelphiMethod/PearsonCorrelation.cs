using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DelphiMethod
{
    // Распределение пирсона
    [Serializable]
    public class PearsonCorrelation
    {
        // Таблица значений
        public double[,] P;
        // Список альф(степеней свободы)
        public List<double> Alphas;
        // Количество строк
        public int Length;

        public PearsonCorrelation(string filename = "P.csv")
        {
            var path = Path.Combine(Path.GetFullPath(@"..\..\"), filename);
            var lines = File.ReadAllLines(path);
            var height = lines.Length;
            var width = lines[1].Split(' ').Length;
            Length = lines.Length - 1;
            P = new double[height, width];

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
