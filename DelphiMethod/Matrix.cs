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
        // sum(q_k * K_i * x_i_j^k)
        public List<decimal> GroupEvaluations =>
            Alternatives.Select(x => x.GroupEvaluation(InitialData.WeightIndicators[0])).ToList();

        public Matrix(List<Alternative> alternatives, InitialData initialData)
        {
            Alternatives = alternatives;
            InitialData = initialData;
        }

        public Matrix(InitialData initialData)
        {
            InitialData = initialData;
            Alternatives = Enumerable.Repeat(new Alternative(initialData.AlternativesCount), initialData.ExpertsCount).ToList();
        }

        public decimal this[int row, int col] => Alternatives[row].Values[col];

        public sealed override string ToString()
        {
            return string.Join("\n", Alternatives.Select(Convert.ToString).ToArray());
        }
    }
}
