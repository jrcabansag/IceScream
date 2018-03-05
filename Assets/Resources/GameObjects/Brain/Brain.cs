using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {
	public Vector3 moveDirection;

	private bool isActive = true;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 5f);
	}

	// Update is called once per frame
	void Update () {
		if (isActive) {
			transform.Rotate (0f, 400f * Time.deltaTime, 400f * Time.deltaTime);
		}
	}

	void OnCollisionEnter(Collision col){
		gameObject.GetComponent<Rigidbody> ().useGravity = true;
		isActive = false;
		if (col.transform.tag == "Player") {
			col.transform.GetComponent<PlayerScript> ().WasHit (moveDirection);
		}
		if (col.transform.tag == "Ground") {
			gameObject.layer = 11;
			Destroy (gameObject, 2f);
		}
	}
}
