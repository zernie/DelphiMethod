using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DelphiMethod
{
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
        public int ExpertsCount;                 // m, Количество экспертов
        public List<Alternative> Alternatives;   // Альтернативы
        // sum(q_k * K_i * x_i_j^k)
        public List<decimal> GroupEvaluations => Alternatives.Select(x => x.GroupEvaluation(Indicator)).ToList();

        // q_k, Коэффициент веса
        public decimal Indicator;

        public Matrix(decimal indicatorNumber)
        {
            Alternatives = new List<Alternative>();
            Indicator = indicatorNumber;
        }

        public Matrix(List<Alternative> alternatives, decimal indicator)
        {
            Alternatives = alternatives;
            Indicator = indicator;
        }

        public Matrix(decimal indicator, int rows, int cols)
        {
            Indicator = indicator;
            Alternatives = Enumerable.Repeat(new Alternative(cols), rows).ToList();
        }

        public decimal this[int row, int col] => Alternatives[row].Values[col];

        public sealed override string ToString()
        {
            return string.Join("\n", Alternatives.Select(Convert.ToString).ToArray());
        }
    }
}
