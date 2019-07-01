/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package cnfstats.Classes;

/**
 *
 * @author dmark
 */
public class Clause {
    	int[] vars;
	int[] literals;
	boolean[] isPos;
	public Clause(int[] literals) {
		this.literals = literals;
		vars = new int[literals.length];
		for(int i=0; i<literals.length; i++) { vars[i] = Math.abs(literals[i]); }
		isPos = new boolean[literals.length];
		for(int i=0; i<literals.length; i++) { isPos[i] = (literals[i] > 0); }
	}
}
