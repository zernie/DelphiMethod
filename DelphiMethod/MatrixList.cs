using System;
using System.Collections.Generic;

namespace DelphiMethod
{
    [Serializable]
    public class MatrixList
    {
        public Config Configuration;
        public List<Matrix> Experts = new List<Matrix>();
        //весовые коэффициенты показателей сравнения альтернатив; 

        public MatrixList(Config configuration)
        {
            Configuration = configuration;
        }

        public Matrix this[int index]
        {
            get => Experts[index];
            set => Experts[index] = value;
        }
    }
}
