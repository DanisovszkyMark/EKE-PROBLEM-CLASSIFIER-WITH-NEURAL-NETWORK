package Classes;

/**
 * @author Danisovszky Márk
 */
public class Sigmoid extends ActivationFunction {

    @Override
    public double Activation(double input) {
        return (1.0 / (1.0 + Math.exp(-input)));
    }

    @Override
    public double DActivation(double input) {
        return (input * (1.0 - input));
    } 
}
