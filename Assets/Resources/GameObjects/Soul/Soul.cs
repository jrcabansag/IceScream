using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour {
	public Vector3 moveDirection;
	public float movementSpeed = 8f;
	private Vector3 localPosition;
	public Transform ghostAnchor;
	private bool isActive = true;
	// Use this for initialization
	void Start () {
		//Destroy (gameObject, 6f);
//		iTween.ScaleTo (gameObject, iTween.Hash ("x", 0.5f, "y", 0.5f, "z", 0.5f, "time", 0.01f, "easeType", iTween.EaseType.easeInOutSine));
//		iTween.ScaleTo (gameObject, iTween.Hash ("x", 1f, "y", 1f, "z", 1f, "time", 0.4f, "easeType", iTween.EaseType.easeInOutSine));
//		iTween.ScaleTo (gameObject, iTween.Hash ("x", 0.5f, "y", 0.5f, "z", 0.5f, "time", 0.4f, "delay", 0.4f, "easeType", iTween.EaseType.easeInOutSine));
//		iTween.ScaleTo (gameObject, iTween.Hash ("x", 1f, "y", 1f, "z", 1f, "time", 0.4f, "delay", 0.8f, "easeType", iTween.EaseType.easeInOutSine));
//		iTween.ScaleTo (gameObject, iTween.Hash ("x", 0.5f, "y", 0.5f, "z", 0.5f, "time", 0.4f, "delay", 1.2f, "easeType", iTween.EaseType.easeInOutSine));
//		iTween.ScaleTo (gameObject, iTween.Hash ("x", 1f, "y", 1f, "z", 1f, "time", 0.4f, "delay", 1.6f, "easeType", iTween.EaseType.easeInOutSine));
		//iTween.PunchScale(gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "z", 0.5f, "time", 2f, "easeType", iTween.EaseType.easeInOutSine));
		//iTween.ScaleTo(gameObject, iTween.Hash("x", 0f, "y", 0f, "z", 0f, "time", 0.01f, "easeType", iTween.EaseType.easeInOutSine));
		iTween.ScaleTo(gameObject, iTween.Hash("x", .3f, "y", .3f, "z", .3f, "time", 0.5f, "delay", 0.05f, "easeType", iTween.EaseType.easeInOutSine));
		iTween.ScaleTo (gameObject, iTween.Hash ("x", 0f, "y", 0f, "z", 0f, "time", 1f, "delay", 5.0f, "easeType", iTween.EaseType.easeInOutSine, "oncomplete", "DestroyOrb", "oncompleteTarget", this.gameObject));
		localPosition = new Vector3 (0f, 3f, 0f);
	}

	void DestroyOrb(){
		Destroy (gameObject);
	}
	// Update is called once per frame
	void FixedUpdate () {
		localPosition += moveDirection * movementSpeed * Time.fixedDeltaTime;
		transform.position = ghostAnchor.position + localPosition;
	}

	void OnCollisionEnter(Collision col){
		isActive = false;
		if (col.transform.tag == "Player") {
			//gameObject.layer = 11;
			col.transform.GetComponent<PlayerScript> ().WasHit (moveDirection);
			iTween.ScaleTo (gameObject, iTween.Hash ("x", 0f, "y", 0f, "z", 0f, "time", 1f, "easeType", iTween.EaseType.easeInOutSine, "oncomplete", "DestroyOrb", "oncompleteTarget", this.gameObject));
		}
	}
}
