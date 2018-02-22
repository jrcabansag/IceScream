using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyEmoteCanvasScript : MonoBehaviour {

	private bool isHidden = false;

	private Transform mainCamera; 
	//private TextMeshProUGUI emoteText;
	// Use this for initialization
	void OnEnable () {
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").transform;
		//emoteText = gameObject.transform.GetChild (0).GetComponent<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
//		Vector3 v = mainCamera.transform.position - transform.position;
//		v.x = v.z = 0.0f;
//		transform.LookAt( mainCamera.transform.position - v ); 
//		transform.Rotate(0,180,0);
	}

	public void SetText(string emote){
		//emoteText.text = emote;
	}

	public void Show(){
		if (isHidden) {
			isHidden = false;
			transform.localScale = Vector3.one;
			transform.rotation = Quaternion.Euler (0f, 0f, 0f);
			Enable ();
		}
	}

	public void Hide(){
		if (!isHidden) {
			iTween.ScaleTo (gameObject, iTween.Hash ("x", 0.3f, "y", 0.3f, "time", 1f, "oncomplete", "Disable"));
			isHidden = true;
		}
	}

	public void HideImmediate(){
		if (!isHidden) {
			Disable ();
			isHidden = true;
		}
	}

	void Disable(){
		gameObject.SetActive (false);
	}

	void Enable(){
		gameObject.SetActive (true);
	}
}
