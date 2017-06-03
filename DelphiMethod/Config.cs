using System;
using System.Collections.Generic;

namespace DelphiMethod
{
    // Настройки
    [Serializable]
    public struct Config
    {
        // Кол-во альтернатив n
        public int N => Alternatives.Count;
        // Кол-во экспертов m
        public int M => Experts.Count;
        // Кол-во показателей l
        public int L => Indicators.Count;

        // Названия альтернатив
        public List<string> Alternatives;
        // Имена экспертов
        public List<string> Experts;
        // Весовые коэффиценты q^k и их названия
        public List<Indicator> Indicators;


        // Индекс уровня значимости критерия α
        public int AlphaIndex;
        // Шкала оценок
        public Range RatingScale;
        public PearsonCorrelation PearsonCorrelationTable;
    }
}
