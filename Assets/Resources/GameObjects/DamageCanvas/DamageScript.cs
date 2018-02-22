using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageScript : MonoBehaviour {

	Text damageText;
	Animator damageAnimator;
	Transform mainCamera;

	// Use this for initialization
	void Awake () {
		damageText = gameObject.transform.GetChild (0).GetComponent<Text>();
		damageAnimator = damageText.GetComponent<Animator> ();
		AnimatorClipInfo[] clipInfo = damageAnimator.GetCurrentAnimatorClipInfo(0);
		Destroy (gameObject, clipInfo [0].clip.length);
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").transform;
	}

	public void SetDamage(int damage){
		damageText.text = "" + damage;
	}

	public void SetPosition(Vector3 position){
		transform.position = position;
	}
		
	// Update is called once per frame
	void Update () {
		Vector3 v = mainCamera.transform.position - transform.position;
		v.x = v.z = 0.0f;
		transform.LookAt( mainCamera.transform.position - v ); 
		transform.Rotate(0,180,0);
	}
}
