using System;
using System.Collections.Generic;
using System.Linq;

namespace DelphiMethod
{
    [Serializable]
    public struct WeightIndicators
    {
        public List<string> Titles;
        public List<double> Values;
        public double Sum => Values.Sum();
        public double Count => Values.Count;
        
        public WeightIndicators(List<string> titles, List<double> values)
        {
            Titles = titles;
            Values = values;
        }

        public double this[int index] => Values[index];
    }
}