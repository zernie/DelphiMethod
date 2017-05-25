using System;

namespace DelphiMethod
{
    // Коэффициент
    [Serializable]
    public struct Indicator
    {
        // название
        public string Title;
        // вес коэффициента
        public double Weight;
        
        public Indicator(string title, double weight)
        {
            Title = title;
            Weight = weight;
        }
    }
}