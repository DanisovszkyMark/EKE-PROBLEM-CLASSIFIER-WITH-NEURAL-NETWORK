/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package cnfstats.Classes;

import java.util.ArrayList;

/**
 *
 * @author dmark
 */
public class DIMACSStatisticsBuilder {
    	int numberOfVariables = 0;
	int numberOfClauses = 0;
	int[] numberOfKClauses = new int[14]; // the 13.th counts 13 or even lengther clauses 
	int numberOfBlackClauses = 0;
	int numberOfWhiteClauses = 0;
	int numberOfDefiniteHornClauses = 0;
	int numberOfStraitClauses = 0;
	int numberOfPositiveLiterals = 0;
	int numberOfNegativeLiterals = 0;
	double ratioOfClausesAndVariables;
	double[] ratioOfKClauses = new double[14];
	double ratioOfBlackClauses;
	double ratioOfWhiteClauses;
	double ratioOfDefiniteHornClauses;
	double ratioOfStraitClauses;
	double ratioOfPositiveLiterals;
	double ratioOfNegativeLiterals;
	boolean mayBePigeonHole;
	boolean mayBeRandom3SAT;
	boolean mayBeRandomAIM;
	boolean mayBeNemesisFormula;
	boolean mayBeDubois;
	public DIMACSStatisticsBuilder(DIMACSReader reader) {
		ArrayList<int[]> cs = reader.cs;
		for(int i=0; i<cs.size(); i++) {
			Clause c = new Clause(cs.get(i));
			addClause(c);
		}
	}
	public DIMACSStatisticsBuilder addClause(Clause c) {
		numberOfClauses++;
		int length = c.vars.length;
		int numberOfPos = 0;
		for(int i=0; i< length; i++) {
			if (c.vars[i] > numberOfVariables) numberOfVariables = c.vars[i];
			if (c.isPos[i]) {
				numberOfPos++;
				numberOfPositiveLiterals++;
			}
			else { numberOfNegativeLiterals++; }
		}
		int numberOfNeg = length - numberOfPos;
		if (length < 13) numberOfKClauses[length]++; else numberOfKClauses[13]++;  
		if (numberOfPos == length) numberOfWhiteClauses++;
		if (numberOfNeg == length) numberOfBlackClauses++;
		if (numberOfPos == 1 && numberOfNeg > 0) numberOfDefiniteHornClauses++;
		if (numberOfNeg == 1 && numberOfPos > 0) numberOfStraitClauses++;
		return this;
	}
	public DIMACSStatisticsBuilder finalizy() {
		ratioOfClausesAndVariables = (double)numberOfClauses / numberOfVariables;
		for(int i=0; i<numberOfKClauses.length; i++) {
			ratioOfKClauses[i] = (double)numberOfKClauses[i] / numberOfClauses;
		}
		ratioOfBlackClauses = (double)numberOfBlackClauses / numberOfClauses;
	    ratioOfWhiteClauses = (double)numberOfWhiteClauses / numberOfClauses;
		ratioOfDefiniteHornClauses = (double)numberOfDefiniteHornClauses / numberOfClauses;
		ratioOfStraitClauses = (double)numberOfStraitClauses / numberOfClauses;
		ratioOfPositiveLiterals = (double)numberOfPositiveLiterals / (numberOfPositiveLiterals + numberOfNegativeLiterals);
		ratioOfNegativeLiterals = (double)numberOfNegativeLiterals / (numberOfPositiveLiterals + numberOfNegativeLiterals);
		mayBePigeonHole = mayItBePigeonHole();
		mayBeRandom3SAT = mayItBeRandom3SAT();
		mayBeRandomAIM = mayItBeRandomAIM();
		mayBeNemesisFormula = mayItBeNemesisFormula();
		mayBeDubois = mayItBeDubois();
		return this;
	}
	private boolean mayItBePigeonHole() {
		int binary = numberOfKClauses[2];
		if (binary != numberOfBlackClauses) return false;
		if (numberOfBlackClauses + numberOfWhiteClauses != numberOfClauses) return false;
		int nonBinary = numberOfClauses - binary;
		if (nonBinary < 4) return false; 
		int k = nonBinary-1;
		k = k > 12 ? 13 : k;
		if (numberOfKClauses[k] != k+1) return false;
		return true;
	}
	private boolean mayItBeRandom3SAT() {
		if (numberOfKClauses[3] != numberOfClauses) return false;
		if (Math.abs(ratioOfClausesAndVariables - 4.267) > 0.3) return false;
		return true;
	}
	private boolean mayItBeRandomAIM() {
		if (numberOfKClauses[2] > 1) return false;
		if (numberOfKClauses[2] + numberOfKClauses[3] != numberOfClauses) return false;
		if (Math.abs(ratioOfPositiveLiterals - 0.5) > 0.2) return false;
		if (Math.abs(ratioOfNegativeLiterals - 0.5) > 0.2) return false;
		if (numberOfPositiveLiterals == numberOfNegativeLiterals) return false;
		return true;
	}
	private boolean mayItBeNemesisFormula() {
		if (ratioOfKClauses[1] > 0.1) return false;	
		if (ratioOfKClauses[1] > 0.1) return false;
		if (ratioOfKClauses[2] < 0.4) return false;
		if (ratioOfKClauses[3] < 0.2) return false;
		if (ratioOfKClauses[2] <= ratioOfKClauses[3]) return false;
		return true;
	}
	
	private boolean mayItBeDubois() {
		if (numberOfKClauses[3] != numberOfClauses) return false;
		if (numberOfBlackClauses != 1) return false;
		if (numberOfPositiveLiterals != numberOfNegativeLiterals) return false;
		return true;
	}
	
	
	public void print() {
		System.out.println("numberOfVariables: " + numberOfVariables);
		System.out.println("numberOfClauses: " + numberOfClauses);
		for(int i=0; i<numberOfKClauses.length; i++) {
			System.out.println("numberOfKClauses["+i+"]: " + numberOfKClauses[i]);
		}
		System.out.println("numberOfBlackClauses: " + numberOfBlackClauses);
		System.out.println("numberOfWhiteClauses: " + numberOfWhiteClauses);
		System.out.println("numberOfDefiniteHornClauses: " + numberOfDefiniteHornClauses);
		System.out.println("numberOfStraitClauses: " + numberOfStraitClauses);
		System.out.println("numberOfPositiveLiterals: " + numberOfPositiveLiterals);
		System.out.println("numberOfNegativeLiterals: " + numberOfNegativeLiterals);
		System.out.println("ratioOfClausesAndVariables: " + ratioOfClausesAndVariables);
		for(int i=0; i<ratioOfKClauses.length; i++) {
			System.out.println("ratioOfKClauses["+i+"]: " + ratioOfKClauses[i]);
		}
		System.out.println("ratioOfBlackClauses: " + ratioOfBlackClauses);
		System.out.println("ratioOfWhiteClauses: " + ratioOfWhiteClauses);
		System.out.println("ratioOfDefiniteHornClauses: " + ratioOfDefiniteHornClauses);
		System.out.println("ratioOfStraitClauses: " + ratioOfStraitClauses);
		System.out.println("ratioOfPositiveLiterals: " + ratioOfPositiveLiterals);
		System.out.println("ratioOfNegativeLiterals: " + ratioOfNegativeLiterals);
		System.out.println("mayBePigeonHole: " + mayBePigeonHole);
		System.out.println("mayBeRandom3SAT: " + mayBeRandom3SAT);
		System.out.println("mayBeRandomAIM: " + mayBeRandomAIM);
		System.out.println("mayBeNemesisFormula: " + mayBeNemesisFormula);
		System.out.println("mayBeDubois: " + mayBeDubois);
	}
}
