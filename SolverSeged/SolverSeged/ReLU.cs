using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverSeged
{
    class ReLU : ActivationStrategy
    {
        public override double Activation(double input)
        {
            if (input < 0) return 0;
            else return input;
        }

        public override double DeActivation(double output)
        {
            if (output < 0) return 0;
            else return 1;
        }

        public override string ToString()
        {
            return "ReLU";
        }
    }

}
