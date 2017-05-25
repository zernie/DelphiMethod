using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DelphiMethod
{
    [Serializable]
    public class PearsonCorrelation
    {
        public double[,] P;
        public List<double> alphas;

        public PearsonCorrelation(string filename = "P.csv")
        {
            var path = Path.Combine(Path.GetFullPath(@"..\..\"), filename);
            var lines = File.ReadAllLines(path);
            var height = lines.Length;
            var width = lines[1].Split(' ').Length;
             P = new double[height, width];

            var firstLine = lines[0].Split(' ');
            alphas = firstLine.Select(Convert.ToDouble).ToList();

            for (var i = 0; i < lines.Length-1; i++)
            {
                var line = lines[i+1].Split(' ');
                for (var j = 0; j < line.Length; j++)
                {
                    P[i, j] = Convert.ToDouble(line[j]);
                }
            }
        }
    }
}
