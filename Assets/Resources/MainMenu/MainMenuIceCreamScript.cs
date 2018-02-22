using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuIceCreamScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		iTween.MoveBy(gameObject, iTween.Hash("y",0.5f,"time", 0.5f,"loopType","pingPong"));
	}

	// Update is called once per frame
	void Update () {
		
	}
}
