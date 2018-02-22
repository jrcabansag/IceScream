using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamPuddle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//transform.rotation = Quaternion.Euler (-90f, Random.Range(0f, 360f), 0f);
		iTween.ScaleTo (gameObject, iTween.Hash ("x", 0.6f, "y", 0.6f, "time", 2.5f, "delay", 0.9f));
		//iTween.ScaleTo (gameObject, iTween.Hash ("z", 0.01f, "time", 3f));
		Destroy (gameObject, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
