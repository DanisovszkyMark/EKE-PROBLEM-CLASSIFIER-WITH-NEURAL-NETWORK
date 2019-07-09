package Classes;

/**
 * @author Danisovszky Márk
 */
public class Layer {
    private int numberOfInput;
    private double[] inputs;
    
    private double[] outputs;
    private int numberOfOutput;
    public double[] getOutputs()
    {
        return outputs;
    }
    
    private double[][] weights;
    
    private ActivationFunction acti;
    
    public double[] feedForward(double[] inputs){
        this.inputs = inputs;
        for (int i = 0; i < this.numberOfOutput; i++) {
            
            this.outputs[i] = 0;
            for (int j = 0; j < this.numberOfInput; j++) {
                outputs[i] += inputs[j] * weights[i][j];
            }
            outputs[i] = acti.Activation(outputs[i]);
        }
        return outputs;
    }
    
    public Layer(double[] inputs, double[] outputs, double[][] weights, ActivationFunction acti){
        this.numberOfInput = inputs.length;
        this.inputs = inputs;
        this.numberOfOutput = outputs.length;
        this.outputs = outputs;
        this.weights = weights;
        this.acti = acti;
    }
}
