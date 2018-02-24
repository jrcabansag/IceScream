using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBreak : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		if (col.transform.tag == "Ground") {
			gameObject.layer = 11;
			//Destroy (gameObject, 5f);
		}
	}
}
