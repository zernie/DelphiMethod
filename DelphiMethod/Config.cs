using System;
using System.Collections.Generic;

namespace DelphiMethod
{
    // Настройки
    [Serializable]
    public struct Config
    {
        // Кол-во альтернатив n
        public int n => Alternatives.Count;
        // Кол-во экспертов m
        public int m => Experts.Count;
        // Кол-во показателей l
        public int l => Indicators.Count;

        // Названия альтернатив
        public List<string> Alternatives;
        // Имена экспертов
        public List<string> Experts;
        // Весовые коэффиценты q^k и их названия
        public List<Indicator> Indicators;

        // Уровень значимости критерия α
        public double Alpha => PearsonCorrelationTable.Alphas[AlphaIndex];

        // Табличное значение x2
        public double x2Alpha => PearsonCorrelationTable.P[n - 2, AlphaIndex];
        // Индекс уровня значимости критерия α
        public int AlphaIndex;
        // Шкала оценок
        public Range RatingScale;
        public PearsonCorrelation PearsonCorrelationTable;
    }
}
