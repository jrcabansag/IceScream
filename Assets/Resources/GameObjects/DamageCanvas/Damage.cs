using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Damage : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		iTween.MoveBy (gameObject, iTween.Hash ("y", 0.6f, "easeType", iTween.EaseType.easeInOutSine, "time", 0.25f));
		iTween.ScaleTo(gameObject, iTween.Hash ("x", 1.2f, "y", 1.2f, "easeType", iTween.EaseType.easeInOutSine, "time", 0.2f));
		iTween.MoveBy (gameObject, iTween.Hash ("y", -0.5f, "easeType", iTween.EaseType.easeInOutSine, "time", 0.4f, "delay", 0.5f));
		iTween.FadeTo (gameObject, iTween.Hash ("alpha", 0.5f, "easeType", iTween.EaseType.easeInOutSine, "time", 0.4f, "delay", 0.5f));
		iTween.ScaleTo (gameObject, iTween.Hash ("x", 0.4f, "y", 0.4f, "easeType", iTween.EaseType.easeInOutSine, "time", 0.2f, "delay", 0.6f, "oncomplete", "Die"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetDamage(int damage){
		transform.Find ("DamagesText").GetComponent<TextMeshProUGUI> ().text = ""+damage;
	}

	void Die(){
		Destroy (gameObject);
	}
}
