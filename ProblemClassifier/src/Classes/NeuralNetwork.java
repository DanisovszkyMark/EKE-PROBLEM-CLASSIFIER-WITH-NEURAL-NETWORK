package Classes;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;

/**
 * @author Danisovszky Márk
 */
public class NeuralNetwork {
    private Layer[] layers;
    
    /**
     * Visszaadja a bemenetre a háló véleményét
     * @param inputs A feldolgozandó bemenet
     * @return double[] mely a háló véleményét tartalmazza
     */
    public double[] feedForward(double[] inputs)
    {
        this.layers[0].feedForward(inputs);
        for (int i = 1; i < this.layers.length; i++) {
            this.layers[i].feedForward(this.layers[i-1].getOutputs());
        }
        
        return this.layers[this.layers.length -1].getOutputs();
    }
    
    /**
     * Létrehozza a neurális hálózatot a megadodd .txt fájl alapján
     * @param filePath A fájl elérési útvonala
     * @throws Exception
     */
    public NeuralNetwork(String filePath)throws Exception{
        File file = new File(filePath);
        BufferedReader reader = new BufferedReader(new FileReader(file));
        
        String[] firstLine = reader.readLine().split(":");
        
        int[] layerBuild = new int[firstLine.length-1];
        for (int i = 0; i < layerBuild.length; i++) {
            layerBuild[i] = Integer.parseInt(firstLine[i]);
        }
        
        ActivationFunction acti = null;
        
        switch (firstLine[firstLine.length-1]) {
            case "Sigmoid":
                acti = new Sigmoid();
                break;
            case "TanH":
                acti = new TanH();
                break;
            case "ReLU":
                acti = new ReLU();
                break;
            default:
                System.out.println("Nem támogatott aktivációs függvény!");
                break;
        }
        
        this.layers = new Layer[layerBuild.length - 1];
        
        String readedLine = reader.readLine();
        String[] lineSplit;
        int counter = 0;
        while(readedLine != null)
        {
            lineSplit = readedLine.split(":");
            
            double[] inputs = new double[Integer.parseInt(lineSplit[0])];
            double[] outputs = new double[Integer.parseInt(lineSplit[1])];
            
            readedLine = reader.readLine();
            lineSplit = readedLine.split(":");
            for (int i = 0; i < outputs.length; i++) {
                outputs[i] = Double.parseDouble(lineSplit[i]);
            }
            
            readedLine = reader.readLine();
            lineSplit = readedLine.split(":");
            for (int i = 0; i < lineSplit.length; i++) {
                inputs[i] = Double.parseDouble(lineSplit[i]);
            }
            
            
            double[][] weights = new double[outputs.length][inputs.length];
            for (int i = 0; i < outputs.length; i++) {
                lineSplit = reader.readLine().split(":");
                for (int j = 0; j < inputs.length; j++) {
                    weights[i][j] = Double.parseDouble(lineSplit[j]);
                }
            }
            
            this.layers[counter] = new Layer(inputs, outputs, weights, acti);
            
            readedLine = reader.readLine();
            counter++;
        }
    }
}