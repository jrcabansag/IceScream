using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

	Transform breakHeart;
	// Use this for initialization
	void Start () {
		breakHeart = ((GameObject)Resources.Load("GameObjects/Hearts/HeartBreak", typeof(GameObject))).transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		if (col.transform.tag == "Ground") {
			//gameObject.GetComponent<Renderer>().material = Resources.Load("Materials/DeadHeart", typeof(Material)) as Material;
//			gameObject.layer = 11;
//			Destroy (gameObject, 2f);
			Transform heartBreak = Instantiate(breakHeart);
			heartBreak.position = transform.position;
			heartBreak.rotation = transform.rotation;
			Destroy(gameObject);
		}
		if (col.transform.tag == "Enemy") {
			//gameObject.GetComponent<Renderer>().material = Resources.Load("Materials/DeadHeart", typeof(Material)) as Material;
			Transform heartBreak = Instantiate(breakHeart);
			heartBreak.position = transform.position;
			heartBreak.rotation = transform.rotation;
			Destroy(gameObject);
		}
	}
}
