using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamScript : MonoBehaviour {

	public float lifeTime = 5f;
	private float createdTime;
	public float powerDivisor = 10f;
	public float powerExp = 1f;
	public bool isActive = true;
	private UI ui;

	void Start () {
		createdTime = Time.time;
		Destroy (gameObject, lifeTime);
		iTween.ScaleTo (gameObject, iTween.Hash ("x", 0.25f, "y", 0.25f, "z", 0.25f, "time", lifeTime));
		ui = GameObject.FindGameObjectWithTag ("UI").GetComponent<UI> ();
	}

	void Update () {
	}

	void OnCollisionEnter(Collision col){
		if (isActive) {
			//print (col.transform.tag);
			gameObject.GetComponent<Rigidbody> ().useGravity = true;
			gameObject.layer = 11;
			isActive = false;
			if (col.gameObject.layer == 12 || col.gameObject.layer == 19) {
				Transform enemy = col.transform;
				Bounds enemyBounds = enemy.GetComponent<Collider> ().bounds;
				float topY = enemyBounds.center.y + enemyBounds.extents.y + 0.2f;
				Vector3 damagePosition = new Vector3 (enemy.position.x, topY + 0.4f, enemy.position.z);
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
					damageScript.SetDamage ((int)(intMax * ui.GetDamageMultiplier ()));
				} else {
					Transform damageCanvas = ((GameObject)Resources.Load ("GameObjects/DamageCanvas/DamageCrit", typeof(GameObject))).transform;
					Transform damager = Instantiate (damageCanvas);
					damager.position = damagePosition;
					DamageCrit damageScript = damager.GetComponent<DamageCrit> ();
					damageScript.SetDamage ((int)(intMax * ui.GetDamageMultiplier ()));
				}
				if (col.transform.tag == "Skeleton" || col.transform.tag == "Zombie") {
					Enemy enemyScript = enemy.GetComponent<Enemy> ();
					enemyScript.Hit ((int)(intMax * ui.GetDamageMultiplier ()), transform.up);
				} else if (col.transform.tag == "Vampire") {
					Vampire vampire = enemy.GetComponent<Vampire> ();
					vampire.WasHit ((int)(intMax * ui.GetDamageMultiplier ()), transform.up);
				} else if (col.transform.tag == "Ghost") {
					Ghost ghost = enemy.GetComponent<Ghost> ();
					ghost.WasHit ((int)(intMax * ui.GetDamageMultiplier ()), transform.up);
				} else if (col.transform.tag == "Witch") {
					Witch witch = enemy.GetComponent<Witch> ();
					witch.WasHit ((int)(intMax * ui.GetDamageMultiplier ()), transform.up);
				}

			} else if (col.gameObject.layer == 25) {
				col.gameObject.GetComponent<Sign> ().Activated ();
			}
			iTween.ScaleTo (gameObject, iTween.Hash ("x", 0.1f, "y", 0.03f, "z", 0.1f, "time", 0.2f));
		}
		if (col.transform.tag == "Ground") {
			Destroy (gameObject, 0.3f);
			gameObject.layer = 11;
		}
	}
}
