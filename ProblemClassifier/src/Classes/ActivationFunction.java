package Classes;

/**
 * Közös típust biztosít az összes aktivációs függvénynek (strategy)
 * @author Danisovszky Márk
 */
public abstract class ActivationFunction {
    public abstract double Activation(double input);
    public abstract double DActivation(double input);
}
