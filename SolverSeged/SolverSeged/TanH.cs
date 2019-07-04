using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverSeged
{
    class TanH : ActivationStrategy
    {
        public override double Activation(double input)
        {
            return Math.Tanh(input);
        }

        public override double DeActivation(double output)
        {
            return 1 - (output * output);
        }

        public override string ToString()
        {
            return "TanH";
        }
    }

}
