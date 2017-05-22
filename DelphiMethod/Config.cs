using System;

namespace DelphiMethod
{
    [Serializable]
    public struct Config
    {
        public int AlternativesCount;
        public int ExpertsCount;
        public int IndicatorsCount;
        public WeightIndicators WeightIndicators;
        public Range RatingScale;
    }
}