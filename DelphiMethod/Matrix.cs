using System;
using System.Collections.Generic;
using System.Linq;

namespace DelphiMethod
{
    public struct Range
    {
        public decimal Start;
        public decimal End;

        public Range(decimal start, decimal end)
        {
            Start = start;
            End = end;
        }

        public bool Includes(decimal value)
        {
            return Start <= value && value <= End;
        }

        public override string ToString()
        {
            return $"{Start} ... {End}";
        }
    }

    public struct InitialData
    {
        public int AlternativesCount;
        public int ExpertsCount;
        public int IndicatorsCount;
        public List<decimal> WeightIndicators;
        public Range RatingScale;
    }

    // Альтернатива
    public class Alternative
    {
        public List<decimal> Values;

        public Alternative(List<decimal> values) => Values = values;
        public Alternative(int count) => Values = new List<decimal>(new decimal[count]);

        public decimal GroupEvaluation(decimal indicator, decimal competenceCoefficient = 0.1M)
        {
            return Values.Select(x => indicator * competenceCoefficient * x).Sum();
        }

        public override string ToString() => string.Join(" ", Values.Select(Convert.ToString).ToArray());
    }

    // Матрица оценок
    public class Matrix
    {
        public InitialData InitialData;
        public List<Alternative> Alternatives;   // Альтернативы
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
