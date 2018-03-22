using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour {
	public string myObject;
	public int xRequirement;
	private int xCount;
	private float lastActivatedTime;
	public float activatedDeltaTime;
	public Material white;
	public Material pink;

	// Use this for initialization
	void Start () {
		xCount = GameData.Level1Rating;
		xCount += GameData.Level2Rating;
		xCount += GameData.Level3Rating;
		xCount += GameData.Level4Rating;
		CheckState ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void CheckState(){
		if (myObject == "Unicone") {
			if (GameData.IceCreamHat == true) {
				transform.Find ("SignBoard").GetComponent<Renderer> ().material = pink;
			} else {
				transform.Find ("SignBoard").GetComponent<Renderer> ().material = white;
			}
		}
		else if (myObject == "Pixiecut") {
			if (GameData.PixieCut == true) {
				transform.Find ("SignBoard").GetComponent<Renderer> ().material = pink;
			} else {
				transform.Find ("SignBoard").GetComponent<Renderer> ().material = white;
			}
		}
		else if (myObject == "Zombify") {
			if (GameData.Zombified == true) {
				transform.Find ("SignBoard").GetComponent<Renderer> ().material = pink;
			} else {
				transform.Find ("SignBoard").GetComponent<Renderer> ().material = white;
			}
		}
		else if (myObject == "Daisy") {
			if (GameData.DaisyHat == true) {
				transform.Find ("SignBoard").GetComponent<Renderer> ().material = pink;
			} else {
				transform.Find ("SignBoard").GetComponent<Renderer> ().material = white;
			}
		}
	}

	public void Activated(){
		if (xCount >= xRequirement) {
			if (lastActivatedTime == 0 || Time.time - lastActivatedTime > activatedDeltaTime) {
				if (myObject == "Unicone") {
					if (GameData.IceCreamHat == true) {
						GameData.IceCreamHat = false;
					} else {
						GameData.IceCreamHat = true;
						GameData.DaisyHat = false;
						GameObject.FindGameObjectWithTag ("DaisySign").GetComponent<Sign> ().CheckState ();
					}
					GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ().ApplyHat ();
				}
				else if (myObject == "Pixiecut") {
					if (GameData.PixieCut == true) {
						GameData.PixieCut = false;
					} else {
						GameData.PixieCut = true;
					}
					GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ().ApplyHair ();
				}
				else if (myObject == "Zombify") {
					if (GameData.Zombified == true) {
						GameData.Zombified = false;
					} else {
						GameData.Zombified = true;
					}
					GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ().ApplySkin ();
				}
				else if (myObject == "Daisy") {
					if (GameData.DaisyHat == true) {
						GameData.DaisyHat = false;
					} else {
						GameData.DaisyHat = true;
						GameData.IceCreamHat = false;
						GameObject.FindGameObjectWithTag ("UniconeSign").GetComponent<Sign> ().CheckState ();
					}
					GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ().ApplyHat ();
				}
				CheckState ();
				lastActivatedTime = Time.time;
			}
		}
	}
}
