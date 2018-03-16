using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {
	public float randomPercent = 0.2f;
	public float defaultCloseDistance = 10f;
	public float defaultSeeDistance = 20f;
	public float defaultViewAngle = 70f;
	public float defaultWillShootDistance = 2f;
	public float walkAngleMax = 35f;
	public float rotationSpeed = 3f;
	public float movementSpeed = 7f;
	public float angryDistanceMultiplier = 2.5f;
	public float angryDuration = 5f;
	public float suspiciousDistance = 5f;
	public float suspiciousDuration = 2f;
	public float suspiciousAfterAwareDuration = 2f;
	public float shootForce = 1500f;
	public float joltedDuration = 1f;
	public float dieForce = 2000f;
	public int currentHealth = 300;
	public int totalHealth = 300;
	public int orbCount = 8;
	private Bar healthBar;
	private static Transform player;
	private static UI ui;
	private Animator animator;
	private Transform emoteCanvas;
	private EnemyEmoteCanvasScript enemyEmoteCanvas;
	private Vector3 playerPosition;
	private Vector3 playerLocalPosition;
	private Quaternion playerLocalAngles;
	private Vector3 playerViewAngles;
	private float playerDistance;
	private string phase = "Idle";
	private bool isWalking = false;
	private float lastAngryTime;
	private float lastSuspiciousTime;
	private float lastJoltedTime;
	private float lastAwareTime;
	private bool isAlive = true;
	private Transform orb;
	private Transform eyes;
	private bool ghostForm = false;
	private Material black;
	private Material ghostEyes;

	void onEnable(){
		//Die ();
	}

	void Start () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player").transform;
		}
		if (ui == null) {
			ui = GameObject.FindGameObjectWithTag ("UI").GetComponent<UI> ();
		}
		animator = transform.GetComponent<Animator> ();
		emoteCanvas = transform.Find("EnemyEmoteCanvas");
		enemyEmoteCanvas = transform.Find("EnemyEmoteCanvas").GetComponent<EnemyEmoteCanvasScript> ();
		enemyEmoteCanvas.HideImmediate ();
		orb = ((GameObject)Resources.Load("GameObjects/Soul/Soul", typeof(GameObject))).transform;
		black = ((Material)Resources.Load ("Materials/Black", typeof(Material)));
		ghostEyes = ((Material)Resources.Load ("Materials/GhostEyes", typeof(Material)));
		eyes = transform.Find ("Eyes");
		Randomify ();
		healthBar = transform.Find ("Bar").GetComponent<Bar>();
	}

	void GetRagdolls(){
	}

	void Die(Vector3 direction){
		Destroy(enemyEmoteCanvas.gameObject);
		gameObject.layer = 11;
		animator.enabled = false;
		transform.Find ("Armature_001/LowerBody").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperBody").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperBody/Head_0").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperBody/CollarBone_L/Shoulder_L/UpperArm_L").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperBody/CollarBone_L/Shoulder_L/UpperArm_L/LowerArm_L").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperBody/CollarBone_R/Shoulder_R/UpperArm_R").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperBody/CollarBone_R/Shoulder_R/UpperArm_R/LowerArm_R").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperLeg_L").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperLeg_L/LowerLeg_L").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperLeg_R").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperLeg_R/LowerLeg_R").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperBody/Head_0").GetComponent<Rigidbody> ().AddForce (direction * 500f);
		transform.Find ("Armature_001/LowerBody/UpperBody").GetComponent<Rigidbody> ().AddForce (direction * 500f);
		transform.Find ("GhostAimHelp").gameObject.layer = 11;
		isAlive = false;
		//Destroy(gameObject);
		ui.UpdateCombo ();
		Invoke ("Gravityfy", 2.5f);
		//Gravityfy ();
	}

	void Gravityfy(){
		transform.Find ("Armature_001/LowerBody").gameObject.layer = 23;
		transform.Find ("Armature_001/LowerBody/UpperBody").gameObject.layer = 23;
		transform.Find ("Armature_001/LowerBody/UpperBody/Head_0").gameObject.layer = 23;
		transform.Find ("Armature_001/LowerBody/UpperBody/CollarBone_L/Shoulder_L/UpperArm_L").gameObject.layer = 23;
		transform.Find ("Armature_001/LowerBody/UpperBody/CollarBone_L/Shoulder_L/UpperArm_L/LowerArm_L").gameObject.layer = 23;
		transform.Find ("Armature_001/LowerBody/UpperBody/CollarBone_R/Shoulder_R/UpperArm_R").gameObject.layer = 23;
		transform.Find ("Armature_001/LowerBody/UpperBody/CollarBone_R/Shoulder_R/UpperArm_R/LowerArm_R").gameObject.layer = 23;
		transform.Find ("Armature_001/LowerBody/UpperLeg_L").gameObject.layer = 23;
		transform.Find ("Armature_001/LowerBody/UpperLeg_L/LowerLeg_L").gameObject.layer = 23;
		transform.Find ("Armature_001/LowerBody/UpperLeg_R").gameObject.layer = 23;
		transform.Find ("Armature_001/LowerBody/UpperLeg_R/LowerLeg_R").gameObject.layer = 23;
		transform.Find ("Armature_001/LowerBody").GetComponent<Rigidbody> ().drag = 15;
		transform.Find ("Armature_001/LowerBody/UpperBody").GetComponent<Rigidbody> ().drag = 15;
		transform.Find ("Armature_001/LowerBody/UpperBody/Head_0").GetComponent<Rigidbody> ().drag = 15;
		transform.Find ("Armature_001/LowerBody/UpperBody/CollarBone_L/Shoulder_L/UpperArm_L").GetComponent<Rigidbody> ().drag = 15;
		transform.Find ("Armature_001/LowerBody/UpperBody/CollarBone_L/Shoulder_L/UpperArm_L/LowerArm_L").GetComponent<Rigidbody> ().drag = 15;
		transform.Find ("Armature_001/LowerBody/UpperBody/CollarBone_R/Shoulder_R/UpperArm_R").GetComponent<Rigidbody> ().drag = 15;
		transform.Find ("Armature_001/LowerBody/UpperBody/CollarBone_R/Shoulder_R/UpperArm_R/LowerArm_R").GetComponent<Rigidbody> ().drag = 15;
		transform.Find ("Armature_001/LowerBody/UpperLeg_L").GetComponent<Rigidbody> ().drag = 15;
		transform.Find ("Armature_001/LowerBody/UpperLeg_L/LowerLeg_L").GetComponent<Rigidbody> ().drag = 15;
		transform.Find ("Armature_001/LowerBody/UpperLeg_R").GetComponent<Rigidbody> ().drag = 15;
		transform.Find ("Armature_001/LowerBody/UpperLeg_R/LowerLeg_R").GetComponent<Rigidbody> ().drag = 15;
	}

	void Randomify(){
		defaultCloseDistance = Random.Range ((1f - randomPercent) * defaultCloseDistance, (1f + randomPercent) * defaultCloseDistance);
		defaultSeeDistance = Random.Range ((1f - randomPercent) * defaultSeeDistance, (1f + randomPercent) * defaultSeeDistance);
		defaultViewAngle = Random.Range ((1f - randomPercent) * defaultViewAngle, (1f + randomPercent) * defaultViewAngle);
		movementSpeed = Random.Range ((1f - randomPercent) * movementSpeed, (1f + randomPercent) * movementSpeed);
	}

	void Update () {
		if (isAlive) {
			GetPlayerPosition ();
			CheckPlayerSeen ();
			SetAnimations ();
			SetGhostForm ();
			if (Input.GetKey ("r") && phase == "Aware") {
				animator.SetTrigger ("IsAttacking");
			}
			SetEmote ();
			ShouldFire ();
			SetHealthBar ();
		}
	}

	void FixedUpdate(){
		if (isAlive) {
			MoveTowardsPlayer ();
		}
	}

	void GetPlayerPosition(){
		playerPosition = player.transform.position;
		playerLocalPosition = playerPosition - transform.position;
		playerDistance = Vector3.Magnitude (playerLocalPosition);
		playerLocalAngles = Quaternion.Euler (0f, Quaternion.LookRotation (playerLocalPosition).eulerAngles.y, 0f);
		playerViewAngles = Quaternion.LookRotation (transform.InverseTransformPoint (playerPosition)).eulerAngles;
	}

	void CheckPlayerSeen(){
		if (player.GetComponent<PlayerScript> ().lives > 0) {
			float closeDistance = defaultCloseDistance;
			float seeDistance = defaultSeeDistance;
			float searchingDistance = defaultSeeDistance;
			float viewAngle = defaultViewAngle;
			if (lastAngryTime != 0 && Time.time - lastAngryTime < angryDuration) {
				viewAngle = 360f;
				searchingDistance = 1000f;
			}
			if (playerDistance < closeDistance) {
				transform.rotation = Quaternion.Slerp (transform.rotation, playerLocalAngles, rotationSpeed * Time.deltaTime);
				phase = "Aware";
			} else if (((playerViewAngles.y < viewAngle || playerViewAngles.y > 360 - viewAngle) && playerDistance < seeDistance)) {
				transform.rotation = Quaternion.Slerp (transform.rotation, playerLocalAngles, rotationSpeed * Time.deltaTime);
				phase = "Aware";
			} else if (((playerViewAngles.y < viewAngle || playerViewAngles.y > 360 - viewAngle) && playerDistance < searchingDistance)) {
				transform.rotation = Quaternion.Slerp (transform.rotation, playerLocalAngles, 1f * Time.deltaTime);
				phase = "Searching";
			} else if (playerDistance < closeDistance + suspiciousDistance || ((playerViewAngles.y < viewAngle || playerViewAngles.y > 360 - viewAngle) && playerDistance < seeDistance + suspiciousDistance)) {
				if (!IsStillAware ()) {
					phase = "Suspicious";
					MakeSuspicious ();
				}
			} else {
				phase = "Idle";
			}
		} else {
			phase = "Idle";
		}
	}

	void MoveTowardsPlayer(){
		if (player.GetComponent<PlayerScript> ().lives > 0) {
			if (phase == "Aware" && (playerViewAngles.y < walkAngleMax || 360f - playerViewAngles.y < walkAngleMax)) {
				transform.Translate (Vector3.Normalize (playerLocalPosition) * movementSpeed * Time.deltaTime, Space.World);
				isWalking = true;
				MakeStillAware ();
			} else if (phase == "Searching" && (playerViewAngles.y < walkAngleMax || 360f - playerViewAngles.y < walkAngleMax) && CheckIfPlayerInSight ()) {
				if (Time.time - lastJoltedTime > joltedDuration) {
					transform.Translate (Vector3.Normalize (playerLocalPosition) * movementSpeed * Time.deltaTime, Space.World);
					isWalking = true;
					MakeStillAware ();
				}
			} else {
				isWalking = false;
			}
		} else {
			isWalking = false;
		}
	}

	void SetAnimations(){
		animator.SetBool ("IsWalking", isWalking);
	}

	void SetGhostForm(){
		if (isWalking && !ghostForm) {
			//print ("GHOST FORM ACTIVATED!");
			ghostForm = true;
			//gameObject.layer = 18;

			//eyes.position = new Vector3 (0f, 0f, 0f);
			//eyes.GetComponent<Renderer> ().enabled = false;
		} else if (!isWalking && ghostForm) {
			//print ("GHOST FORM DEACTIVATED!");
			ghostForm = false;
			//gameObject.layer = 12;
			//eyes.GetComponent<Renderer> ().enabled = true;
		}
	}

	public void MakeAngry(){
		lastAngryTime = Time.time;
	}

	public void MakeSuspicious(){
		lastSuspiciousTime = Time.time;
	}

	public void MakeJolted(){
		lastJoltedTime = Time.time;
	}

	public void MakeStillAware(){
		lastAwareTime = Time.time;
	}

	bool IsStillAware(){
		return lastAwareTime != 0f && Time.time - lastAwareTime < suspiciousAfterAwareDuration;
	}

	bool IsAngry(){
		return lastAngryTime != 0 && Time.time - lastAngryTime < angryDuration;
	}

	void SetEmote(){
		if (phase == "Aware" || phase == "Searching") {
			enemyEmoteCanvas.HideImmediate ();
		} else if (lastSuspiciousTime != 0f && Time.time - lastSuspiciousTime < suspiciousDuration) {
			enemyEmoteCanvas.Show ();
		} else if (lastAwareTime != 0f && Time.time-lastAwareTime < suspiciousAfterAwareDuration){
		} else {
			enemyEmoteCanvas.HideImmediate();
		}
	}



	public void WasHit(int damage, Vector3 direction){
		if (isAlive) {
			currentHealth -= damage;
			if (currentHealth <= 0) {
				Die (direction);
				player.GetComponent<PlayerScript> ().BoostEnergy ();
				Destroy (gameObject, 9f);
			} else if (phase != "Aware" && phase != "Searching") {
				animator.SetTrigger ("IsHit");
				MakeJolted ();
				MakeAngry ();
			} else {
				MakeAngry ();
			}
			UpdateHealth ();
		}
	}

	void ShouldFire(){
		if (phase == "Aware") {
			float angryAdd = 0f;
			if (lastAngryTime != 0 && Time.time - lastAngryTime < angryDuration) {
				angryAdd = 40f;
			}
			if (playerDistance < defaultWillShootDistance) {
				animator.SetTrigger ("IsAttacking");
			}
			if (playerDistance < defaultCloseDistance/2) {
				if (Random.Range (0f, 100f) < (55f + angryAdd) * Time.deltaTime) {
					animator.SetTrigger ("IsAttacking");
				}
			} else if (playerDistance < defaultSeeDistance) {
				if (Random.Range (0f, 100f) < (30f + angryAdd) * Time.deltaTime) {
					animator.SetTrigger ("IsAttacking");
				}
			} else if (Random.Range (0f, 100f) < (15f + angryAdd) * Time.deltaTime) {
				animator.SetTrigger ("IsAttacking");
			}
		}
	}

	void FireOrbs(){
//		if (isWalking) {
//			ShootOrb (Vector3.Normalize (transform.forward), 1500f);
//		} else {
		for (int x = 0; x < orbCount; x++) {
				Vector3 fireDirection = Quaternion.Euler (0f, ((float)360 / orbCount * x), 0) * transform.forward;
				ShootOrb (Vector3.Normalize (fireDirection), 800f);
			}
//		}
	}

	void ShootOrb(Vector3 direction, float shootForce){
		Quaternion rot = Quaternion.LookRotation (direction);
		Transform orbProjectile = Instantiate (orb);
		orbProjectile.GetComponent<Soul> ().ghostAnchor = transform;
		orbProjectile.rotation = rot;
		//Vector3 orbPosition = new Vector3 (transform.position.x, 3f, transform.position.z);
		//if (isWalking) {
		//	orbPosition = orbPosition + 3f * Vector3.Normalize (transform.forward);
		//}
		//orbProjectile.transform.position = orbPosition;
		//orbProjectile.GetComponent<Rigidbody> ().AddForce (direction * shootForce);
		orbProjectile.GetComponent<Soul> ().moveDirection = direction;
		orbProjectile.GetComponent<Soul> ().activated = true;
	}

	bool CheckIfPlayerInSight(){
		Ray sight = new Ray (transform.position, playerLocalPosition);
		RaycastHit hit;
		bool didHit = Physics.Raycast (sight, out hit, 1000f);
		if (didHit && hit.transform.tag == "Player") {
			return true;
		} else {
			return false;
		}
	}

	void UpdateHealth(){
		if (currentHealth <= 0) {
			currentHealth = 0;
			healthBar.Die ();
		} else {
			healthBar.SetValue (currentHealth * 1f / totalHealth);
		}
	}

	void SetHealthBar(){
		if (IsAngry ()) {
			healthBar.Show ();
		} else {
			healthBar.Hide ();
		}
	}
}
