using System;
using System.Collections.Generic;

namespace DelphiMethod
{
    [Serializable]
    public class MatrixList
    {
        // Конфигурация
        public Config Configuration;
        
        // матрицы рангов
        public List<Matrix> Matrices = new List<Matrix>();
        //весовые коэффициенты показателей сравнения альтернатив; 

        public MatrixList(Config configuration)
        {
            Configuration = configuration;
        }

        public Matrix this[int index]
        {
            get => Matrices[index];
            set => Matrices[index] = value;
        }
    }
}
