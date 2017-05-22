using System.Collections.Generic;

namespace DelphiMethod
{
    public class MatrixList
    {
        public InitialData InitialData;
        public List<Matrix> Experts = new List<Matrix>();
        //весовые коэффициенты показателей сравнения альтернатив; 
        
        public MatrixList(InitialData initialData)
        {
            InitialData = initialData;
        }

        public Matrix this[int index]
        {
            get => Experts[index];
            set => Experts[index] = value;
        }
    }
}
