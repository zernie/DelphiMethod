using System;
using System.Collections.Generic;
using System.Linq;

namespace DelphiMethod
{
    // Матрица рангов
    public class Matrix
    {
        public InitialData InitialData;
        public List<Alternative> Alternatives;
        public List<Expert> Experts;
        public int Indicator;

        // sum(q_k * K_i * x_i_j^k)
        public List<decimal> GroupEvaluations =>
            Alternatives.Select(x => x.GroupEvaluation(InitialData.WeightIndicators[Indicator])).ToList();

        public Matrix(List<Alternative> alternatives, InitialData initialData, int indicator)
        {
            Alternatives = alternatives;
            InitialData = initialData;
            Indicator = indicator;
        }

        public Matrix(InitialData initialData, int indicator)
        {
            InitialData = initialData;
            Indicator = indicator;
            Alternatives = Enumerable.Repeat(new Alternative(initialData.AlternativesCount), initialData.ExpertsCount).ToList();
        }

        public decimal this[int row, int col] => Alternatives[row].Values[col];

        public sealed override string ToString()
        {
            return string.Join("\n", Alternatives.Select(Convert.ToString).ToArray());
        }
    }

    public class Expert
    {
    }
}
