package Classes;

/**
 * @author Danisovszky Márk
 */
public class TanH extends ActivationFunction {

    @Override
    public double Activation(double input) {
        return Math.tanh(input);
    }

    @Override
    public double DActivation(double input) {
        return 1 - (input * input);
    }
}
