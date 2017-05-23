using System;
using System.Collections.Generic;
using System.Linq;

namespace DelphiMethod
{
    // Веса коэффициентов
    [Serializable]
    public struct WeightIndicators
    {
        // массив названий
        public List<string> Titles;
        // Массив значений
        public List<double> Values;
        // Сумма значений
        public double Sum => Values.Sum();
        // Кол-во элементов
        public int Count => Values.Count;
        
        public WeightIndicators(List<string> titles, List<double> values)
        {
            Titles = titles;
            Values = values;
        }

        public double this[int index] => Values[index];
    }
}