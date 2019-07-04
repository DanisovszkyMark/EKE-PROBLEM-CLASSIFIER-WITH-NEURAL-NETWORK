using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverSeged
{
    class LReLU : ActivationStrategy
    {
        public override double Activation(double input)
        {
            if (input < 0) return 0.01 * input;
            else return input;
        }

        public override double DeActivation(double output)
        {
            if (output < 0) return 0.01 * output;
            else return 1;
        }

        public override string ToString()
        {
            return "LReLU";
        }
    }

}
