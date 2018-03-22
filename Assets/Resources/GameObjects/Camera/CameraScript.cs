using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour {

	private Transform player;
	public Vector3 offset = new Vector3 (0f, 10f, -18f);
	public Vector3 velocity = Vector3.one;
	public float timeToReach = 0.2f;
	public Quaternion defaultRotation;
	public float currentDegree = 0f;
	public float rotateSpeed = 5f;
	public float defaultXRotation = 23.5f;
	public float smoothFactor = 5f;
	public float rotateSmoothFactor = 5f;
	public float maxDegree = 3f;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform.Find ("Armature_001/LowerBody");
		transform.position = player.position + offset;
		//player = GameObject.FindGameObjectWithTag ("Player").transform.Find ("Armature_001/LowerBody/UpperBody/Head");
		defaultRotation = transform.rotation;


	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey (KeyCode.LeftControl)) {
//			Vector3 vectorOffset = new Vector3 (0f, 10f, 0f);
//			Vector3 finalPosition = player.position+vectorOffset;
//			transform.rotation = Quaternion.Euler (90f, 0f, 0f);
//			Vector3 currentPosition = Vector3.SmoothDamp (transform.position, finalPosition, ref velocity, timeToReach);
//			transform.position = currentPosition;
			Camera.main.orthographic = true;
//			Camera.main.orthographicSize = 20f;
		} else {
			Camera.main.orthographic = false;
		}
//		} else if (Input.GetKeyUp(KeyCode.Escape)){
//			SceneManager.LoadScene ("MainMenu");
//		}
//		if (Input.GetMouseButton(1)) {
//			//transform.LookAt(target);
//			//transform.RotateAround (player.position, Vector3.up, rotateSpeed);
//			float addDegree = Input.GetAxis("Mouse X")*rotateSpeed;
//			if (addDegree > maxDegree) {
//				addDegree = maxDegree;
//			}
//			if (addDegree < -maxDegree) {
//				addDegree = -maxDegree;
//			}
//			currentDegree += addDegree;
//
//
//		}
//
			float currentRadians = currentDegree/180f*Mathf.PI;
			//Vector3 currentOffset = new Vector3 (-20f*Mathf.Sin (currentRadians), 10f, -20f*Mathf.Cos (currentRadians));
			Vector3 finalPosition = player.position + offset;
			//Vector3 currentPosition = Vector3.SmoothDamp (transform.position, finalPosition, ref velocity, timeToReach);
			Vector3 currentPosition = Vector3.Slerp(transform.position, finalPosition, smoothFactor*Time.fixedDeltaTime);
			transform.position = currentPosition;

		Quaternion newRotation = Quaternion.Euler (defaultXRotation, currentDegree, 0f);
			//Quaternion lookTo = Quaternion.LookRotation (player.position - transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, newRotation, rotateSmoothFactor * Time.fixedDeltaTime);
		//transform.rotation = Quaternion.Euler (defaultXRotation, transform.rotation.eulerAngles.y, 0f);
			

		//transform.RotateAround (player.position, Vector3.up);
//		if (Input.GetMouseButton (1)) {
//			Vector3 lookPosition = new Vector3 (player.position.x, transform.position.y, player.position.z);
//			transform.LookAt (lookPosition, Vector3.up);
//			transform.rotation = Quaternion.Euler (defaultXRotation, transform.rotation.eulerAngles.y, 0f);
//		}
	}
}
