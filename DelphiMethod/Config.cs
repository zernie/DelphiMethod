using System;
using System.Collections.Generic;

namespace DelphiMethod
{
    // Конфигурация
    [Serializable]
    public struct Config
    {
        // Кол-во альтернатив n
        public int AlternativesCount;
        // Кол-во экспертов m
        public int ExpertsCount;
        // Кол-во показателей l
        public int IndicatorsCount;
        // Весовые коэффиценты q_k
        public List<Indicator> Indicators;
        // Шкала оценок
        public Range RatingScale;
    }
}