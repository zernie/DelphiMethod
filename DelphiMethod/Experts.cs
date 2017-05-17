using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelphiMethod
{
    class Alternative
    {
        // оценка
        public decimal Value;

        public Alternative(decimal value)
        {
            Value = value;
        }
    }

    class Expert
    {
        public List<Alternative> Alternatives;
    }

    class Matrix
    {
        public List<Expert> Experts;

        public Matrix(List<Expert> experts)
        {
            Experts = experts;
        }
    }
}
