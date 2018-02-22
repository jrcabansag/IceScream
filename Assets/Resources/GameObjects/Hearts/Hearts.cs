using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearts : MonoBehaviour {

	private Transform mainCamera;
	private Transform rightHeart;
	private Transform middleHeart;
	private Transform leftHeart;
	private int lives = 3;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").transform;
		rightHeart = transform.Find ("RightHeart");
		middleHeart = transform.Find ("MiddleHeart");
		leftHeart = transform.Find ("LeftHeart");
	}
	
	// Update is called once per frame
	void Update () {
//		Vector3 v = mainCamera.transform.position - transform.position;
//		v.x = v.z = 0.0f;
//		transform.LookAt( mainCamera.transform.position - v ); 
//		transform.Rotate(0,180,0);
		transform.rotation = Quaternion.Euler (0f, 0f, 0f);
	}

	public void DropLife(){
		if (lives == 3) {
			DropHeart (rightHeart);
			leftHeart.localPosition = new Vector3(-.65f/2f, leftHeart.localPosition.y, 0f);
			middleHeart.localPosition = new Vector3(.65f/2f, leftHeart.localPosition.y, 0f);
		}
		if (lives == 2) {
			DropHeart (middleHeart);
			leftHeart.localPosition = new Vector3(0f, leftHeart.localPosition.y, 0f);
		}
		if (lives == 1) {
			DropHeart (leftHeart);
		}
		lives -= 1;
	}

	void DropHeart(Transform heart){
		heart.GetComponent<Rigidbody> ().isKinematic = false;
		heart.GetComponent<Rigidbody> ().AddTorque (100f, 100f, 100f);
		heart.SetParent (null);
	}

	public void Show(){
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in renderers) {
			renderer.enabled = true;
		}
	}

	public void Hide(){
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in renderers) {
			renderer.enabled = false;
		}
	}
}
