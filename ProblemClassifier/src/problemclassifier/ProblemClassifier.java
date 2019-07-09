package problemclassifier;

import Classes.NeuralNetwork;
import java.util.Arrays;

/**
 * @author Danisovszky Márk
 */
public class ProblemClassifier {

    public static void main(String[] args) throws Exception {
        NeuralNetwork nn = new NeuralNetwork("C:\\Users\\dmark\\Desktop\\teacher1\\NeuralNetwork_01\\bin\\Debug\\Sigmoid0.txt");
        
        System.out.println("Elvárt: GCP (7. osztály)");
        double[] results = nn.feedForward(new double[]{2125,66272,0,0,66147,0,0,0,0,0,0,0,0,0,0,125,66147,125,0,0,2125,132294,31.186823529411765,0.0,0.0,0.9981138338966683,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0018861661033317237,0.9981138338966683,0.0018861661033317237,0.0,0.0,0.015808777033008727,0.9841912229669912,0.0,0.0,0.0,0.0,0.0});
        int maxI = 0;
        for (int i = 1; i < results.length; i++) {
            if(results[maxI] < results[i]) maxI = i;
        }
        
        System.out.println(maxI+1 + ". osztály -> " + results[maxI]);
    }
    
}
