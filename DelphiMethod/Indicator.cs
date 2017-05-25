using System;

namespace DelphiMethod
{
    // Показатель
    [Serializable]
    public struct Indicator
    {
        // Название
        public string Title;
        // Вес коэффициента
        public double Weight;
        
        public Indicator(string title, double weight)
        {
            Title = title;
            Weight = weight;
        }
    }
}