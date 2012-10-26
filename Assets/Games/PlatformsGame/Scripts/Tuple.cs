using UnityEngine;
using System.Collections;

public class Tuple<F,S> {

	private F first;
	private S second;
	
	public Tuple(F first1, S second1){
		first = first1;
		second = second1;
	}
	
	public F First(){
		return first;
	}
	
	public S Second(){
		return second;
	}
}
