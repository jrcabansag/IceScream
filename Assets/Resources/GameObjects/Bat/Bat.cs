using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour {
	private static Transform player;
	public float rotationSpeed = 3f;
	public float movementSpeed = 7f;
	public float minRotationSpeed = 1.5f;
	public float rotationDecaySpeed = 2f;
	public bool positiveRotation = true;
	private Animator animator;
	private bool isAlive = true;

	// Use this for initialization
	void Start () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player").transform;
		}
		animator = transform.GetComponent<Animator> ();
	}

	void Die(){
		//animator.enabled = false;
		isAlive = false;
		transform.GetComponent<Rigidbody> ().useGravity = true;
		gameObject.layer = 11;
		animator.speed = 0.4f;
		Destroy (gameObject, 2f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isAlive) {
			Vector3 playerPosition = player.transform.position;
			Vector3 playerLocalPosition = playerPosition - transform.position;
			playerLocalPosition.y = 0;
			Quaternion playerLocalAngles = Quaternion.Euler (0f, Quaternion.LookRotation (playerLocalPosition).eulerAngles.y, 0f);
			//transform.Translate (Vector3.Normalize (playerLocalPosition) * movementSpeed * Time.deltaTime, Space.World);
			Vector3 myForward = transform.forward;
			//print (myForward);
			myForward.y = 0f;
			transform.Translate (Vector3.Normalize (myForward) * movementSpeed * Time.deltaTime, Space.World);
			transform.rotation = Quaternion.Slerp (transform.rotation, playerLocalAngles, rotationSpeed * Time.deltaTime);
			if (rotationSpeed > 0) {
				rotationSpeed -= rotationDecaySpeed * Time.deltaTime;
				if (rotationSpeed < 0) {
					rotationSpeed = 0;
				}
			}
			if (rotationSpeed <= 0 && positiveRotation == true) {
				positiveRotation = false;
				Destroy (gameObject, 5f);
			}
		} else {
			Quaternion deathAngle = Quaternion.Euler (-200f, 0f, 0f);
			transform.rotation = Quaternion.Slerp (transform.rotation, deathAngle, rotationSpeed * Time.deltaTime);
		}
	}

	void OnCollisionEnter(Collision col){
		if (col.transform.tag == "Player") {
			Vector3 myForward = transform.forward;
			myForward.y = 0f;
			col.transform.GetComponent<PlayerScript> ().HitEnemy (Vector3.Normalize (myForward), 1000f);
			Destroy (gameObject);
		} else if (col.gameObject.layer == 21) {
			Die ();
		}
	}
}
