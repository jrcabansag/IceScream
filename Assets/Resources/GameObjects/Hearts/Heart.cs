using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		if (col.transform.tag == "Ground") {
			//gameObject.GetComponent<Renderer>().material = Resources.Load("Materials/DeadHeart", typeof(Material)) as Material;
			gameObject.layer = 11;
			Destroy (gameObject, 2f);
		}
		if (col.transform.tag == "Enemy") {
			//gameObject.GetComponent<Renderer>().material = Resources.Load("Materials/DeadHeart", typeof(Material)) as Material;
		}
	}
}
