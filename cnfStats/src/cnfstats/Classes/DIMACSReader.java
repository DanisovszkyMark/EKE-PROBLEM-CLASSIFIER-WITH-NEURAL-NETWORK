/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package cnfstats.Classes;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;

/**
 *
 * @author dmark
 */
public class DIMACSReader {
    	int numberOfVariables;
	int numberOfClauses;
	ArrayList<int[]> cs = new ArrayList<>();
	ArrayList<Integer> units = new ArrayList<>();
	int[] copyBuffer;
	
	public DIMACSReader(String fileName) {
		try {
			BufferedReader file = new BufferedReader(new FileReader(fileName));
			String clause = file.readLine();
			//System.out.println(clause);
			while (clause.charAt(0) != 'p') {
				clause = file.readLine();
				//System.out.println(clause);
			}
			readPLine(clause);
			initDataStructure();
			clause = file.readLine();
			//System.out.println(clause);
			while (clause != null) {
				addCNFClause(clause);
				clause = file.readLine();
				//System.out.println(clause);
			}
			file.close();
		} catch (IOException e) { System.out.println(e); System.exit(-1); }
	}
	private void readPLine(String cnfClause) {
		//System.out.println("cnfClause: " + cnfClause);
		int i1 = 6;
		int i2 = cnfClause.indexOf(" ", i1);
		//System.out.println("i1: " + i1);
		//System.out.println("i2: " + i2);
		//System.out.println(cnfClause.substring(i1, i2));
		//System.out.println(cnfClause.substring(i2+1, cnfClause.length()));
		numberOfVariables = Integer.parseInt(cnfClause.substring(i1, i2));
		while(cnfClause.charAt(i2) == ' ') i2++;
		int i3 = cnfClause.indexOf(" ", i2);
		if (i3==-1) i3 = cnfClause.length();
		numberOfClauses = Integer.parseInt(cnfClause.substring(i2, i3));
		//System.out.println("numberOfVariables: " + numberOfVariables);
		//System.out.println("numberOfClauses: " + numberOfClauses);
	}
	private void initDataStructure() {
		copyBuffer = new int[numberOfVariables];
	}
	private void addCNFClause(String cnfClause) {
		if (cnfClause.length() == 0 ||
			cnfClause.charAt(0) == '0' || 
			cnfClause.charAt(0) == 'c' ||
			cnfClause.charAt(0) == '%') { return; }
		//System.out.println("cnfClause: " + cnfClause);
		int i = 0;
		int i1 = 0;
		cnfClause = cnfClause.replace("\t", " ");
		while(i1 < cnfClause.length())  {
			int i2 = cnfClause.indexOf(" ", i1);
			if (i1 == i2) { i1++; continue; }
			if (i2 == -1) break;
			String lit = cnfClause.substring(i1, i2);
			i1 = i2;
			int literal = Integer.parseInt(lit);
			if (literal == 0) break;
			copyBuffer[i] = literal;
			i++;
		}
		int[] literals = new int[i];
		for(int index=0; index<i; index++) { 
			literals[index] = copyBuffer[index];
		}
		cs.add(literals);
		if (literals.length == 1 && !units.contains(literals[0])) { 
			units.add(literals[0]);
			//System.out.println("unit has been found: " + literals[0]);
			//System.out.println("number of units: " + units.size());
		}
	}
}
