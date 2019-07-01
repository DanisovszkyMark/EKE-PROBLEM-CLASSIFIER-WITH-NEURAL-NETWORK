/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package cnfstats;

import cnfstats.Classes.DIMACSReader;
import cnfstats.Classes.DIMACSStatisticsBuilder;

/**
 *
 * @author dmark
 */
public class CnfStats {

    /**
     * @param args the command line arguments
     */
    private static String fileName = "";
        
    public static void main(String[] args) {

        try {
            if (args.length == 0 || (args.length == 1 && args[0].equals("help")) || (args.length == 1 && args[0].equals("-help"))) {
                fileName = "hole7.cnf";
            }
            if (args.length >= 1) { fileName = args[0]; }
            if (args.length >= 2) {
                System.err.println("Too many parameters! Please read the help:");
                printHelp();
                return;
            }
        }
        catch(Exception e) {
            System.err.println(e);
            System.err.println("Parameter error! Please read the help:");
            printHelp();
            return;
        }
        DIMACSStatisticsBuilder sb = new DIMACSStatisticsBuilder(new DIMACSReader(fileName));
        sb.finalizy();
        sb.print();
    }
    
    private static void printHelp() {
        System.out.println("Usage: java CnfStat cnf_file_name");
        System.out.println("Example: java CnfStat hole7.cnf");
        System.out.println("The parameter 'cnf_file_name' is a file in DIMACS format containing a CNF SAT problem.");
        System.out.println("One can find CNF files here: https://www.cs.ubc.ca/~hoos/SATLIB/benchm.html .");	
    }
}
