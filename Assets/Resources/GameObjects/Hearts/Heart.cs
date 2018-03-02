using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

	private Transform breakHeart;
	public bool isDead = false;
	private bool broken = false;
	// Use this for initialization
	void Start () {
		breakHeart = ((GameObject)Resources.Load("GameObjects/Hearts/HeartBreak", typeof(GameObject))).transform;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		if (isDead && !broken) {
			if (col.gameObject.layer != 0 || col.transform.tag == "Ground") {
				broken = true;
				Transform heartBreak = Instantiate (breakHeart);
				heartBreak.position = transform.position;
				heartBreak.rotation = transform.rotation;
				Destroy (gameObject);
			}
		}
	}
}
