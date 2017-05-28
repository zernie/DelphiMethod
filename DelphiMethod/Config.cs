using System;
using System.Collections.Generic;

namespace DelphiMethod
{
    // Настройки
    [Serializable]
    public struct Config
    {
        // Кол-во альтернатив n
        public int AlternativesCount;
        // Кол-во экспертов m
        public int ExpertsCount;
        // Весовые коэффиценты q^k и их названия
        public List<Indicator> Indicators;
        // Кол-во показателей l
        public int IndicatorsCount => Indicators.Count;
        // Шкала оценок
        public Range RatingScale;
    }
}
