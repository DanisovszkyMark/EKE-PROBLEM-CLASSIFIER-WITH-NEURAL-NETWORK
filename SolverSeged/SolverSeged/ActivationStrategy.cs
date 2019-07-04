using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverSeged
{
    public abstract class ActivationStrategy
    {
        public abstract double Activation(double input);

        public abstract double DeActivation(double output);

        public abstract override string ToString();
    }
}
