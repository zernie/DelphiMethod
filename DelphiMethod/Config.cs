using System;

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
        // Кол-во показателей k
        public int IndicatorsCount;
        // Весовые коэффиценты q_k
        public WeightIndicators WeightIndicators;
        // Шкала оценок
        public Range RatingScale;
    }
}