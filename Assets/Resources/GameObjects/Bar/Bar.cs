using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {

	Transform mainCamera;
	Transform coloredBar;
	Transform backgroundBar;
	Transform parent;
	float currentXScale;
	Vector3 offset;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").transform;
		coloredBar = transform.Find ("ColoredBar");
		backgroundBar = transform.Find ("BackgroundBar");
		currentXScale = transform.localScale.x;
		parent = transform.parent;
		offset = transform.position-parent.position;
		//print ("OFFSET IS "+offset);

	}

	// Update is called once per frame
	void Update () {
		//Vector3 v = mainCamera.transform.position - transform.position;
		//v.x = v.z = 0.0f;
		//transform.LookAt( mainCamera.transform.position - v ); 
		//transform.Rotate(0,180,0);
		transform.rotation = Quaternion.Euler (0f, 0f, transform.rotation.eulerAngles.z);
		transform.position = parent.position+offset;
	}

	public void SetValue(float value){
		coloredBar.localScale = new Vector3 (value, 1f, 1f);
		coloredBar.localPosition = new Vector3 (-backgroundBar.GetComponent<BoxCollider> ().size.x/2+coloredBar.GetComponent<BoxCollider> ().size.x/2*value, 0f, 0f);
	}

	public void Die(){
		Destroy (gameObject);
	}

	public void Hide(){
		coloredBar.GetComponent<Renderer> ().enabled = false;
		backgroundBar.GetComponent<Renderer> ().enabled = false;
	}

	public void Show(){
		coloredBar.GetComponent<Renderer> ().enabled = true;
		backgroundBar.GetComponent<Renderer> ().enabled = true;
	}
}
