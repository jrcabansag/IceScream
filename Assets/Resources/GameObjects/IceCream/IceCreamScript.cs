﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamScript : MonoBehaviour {

	public float lifeTime = 5f;
	private float createdTime;
	public float powerDivisor = 10f;
	public float powerExp = 1f;
	private bool isActive = true;

	void Start () {
		createdTime = Time.time;
		Destroy (gameObject, lifeTime);
		iTween.ScaleTo (gameObject, iTween.Hash ("x", 0.25f, "y", 0.25f, "z", 0.25f, "time", lifeTime));
	}

	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		if (isActive) {
			//print (col.transform.tag);
			gameObject.GetComponent<Rigidbody> ().useGravity = true;
			isActive = false;
			if (col.transform.tag == "Enemy") {
				Transform enemy = col.transform;
				SkeletonScript skeletonScript = enemy.GetComponent<SkeletonScript> ();
				Bounds enemyBounds = enemy.GetComponent<Collider> ().bounds;
				float topY = enemyBounds.center.y + enemyBounds.extents.y+0.2f;
				Vector3 damagePosition = new Vector3 (enemy.position.x, topY+0.4f, enemy.position.z);
				float timeDelta = Time.time - createdTime;
				int intMax = (int)(powerDivisor / Mathf.Pow (timeDelta, powerExp));
				if (intMax > 100) {
					intMax = 100;
				}

				if (intMax < 50) {
					Transform damageCanvas = ((GameObject)Resources.Load ("GameObjects/DamageCanvas/Damage", typeof(GameObject))).transform;
					Transform damager = Instantiate (damageCanvas);
					damager.position = damagePosition;
					Damage damageScript = damager.GetComponent<Damage> ();
					damageScript.SetDamage (intMax);
				} else {
					Transform damageCanvas = ((GameObject)Resources.Load ("GameObjects/DamageCanvas/DamageCrit", typeof(GameObject))).transform;
					Transform damager = Instantiate (damageCanvas);
					damager.position = damagePosition;
					DamageCrit damageScript = damager.GetComponent<DamageCrit> ();
					damageScript.SetDamage (intMax);
				}
				skeletonScript.WasHit (intMax, transform.up);
			}
			if (col.transform.tag == "Obstacle" || col.transform.tag == "Enemy") {
				iTween.ScaleTo (gameObject, iTween.Hash ("x", 0.1f, "y", 0.03f, "z", 0.1f, "time", 0.2f));
			} else {
				print ("WONT SCALE BECAUSE TAG IS " + col.transform.tag);
			}
		}
		if (col.transform.tag == "Ground") {
			Destroy (gameObject, 0.3f);
			gameObject.layer = 11;
		}
	}
}