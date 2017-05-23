using System;

namespace DelphiMethod
{
    // Шкала оценок
    [Serializable]
    public struct Range
    {
        // начало
        public double Start;
        // конец
        public double End;

        public Range(double start, double end)
        {
            Start = start;
            End = end;
        }

        // Включает ли шкала оценок значение?
        public bool Includes(double value)
        {
            return Start <= value && value <= End;
        }

        public override string ToString()
        {
            return $"{Start}...{End}";
        }
    }
}