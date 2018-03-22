using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour {
	public string myObject;
	public int xRequirement;
	private int xCount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		print (col.gameObject.layer);
		if (col.gameObject.layer == 9) {
			print ("HIT BY ICE CREAM!");
		}
	}
}
