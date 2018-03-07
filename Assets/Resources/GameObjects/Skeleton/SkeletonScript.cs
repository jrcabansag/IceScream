using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonScript : MonoBehaviour {
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
	private Transform bone;

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
//		Transform emoteCanvas = ((GameObject)Resources.Load("GameObjects/EnemyEmoteCanvas/EnemyEmoteCanvas", typeof(GameObject))).transform;
//		Transform myEmoteCanvas = Instantiate (emoteCanvas);
//		myEmoteCanvas.SetParent (transform);
		//myEmoteCanvas.localPosition = new Vector3 (0f, 7.3f, 0f);
		emoteCanvas = transform.Find("EnemyEmoteCanvas");
		enemyEmoteCanvas = transform.Find("EnemyEmoteCanvas").GetComponent<EnemyEmoteCanvasScript> ();
		enemyEmoteCanvas.HideImmediate ();
		bone = ((GameObject)Resources.Load("GameObjects/Bone/Bone", typeof(GameObject))).transform;
		Randomify ();
		healthBar = transform.Find ("Bar").GetComponent<Bar>();
	}

	void GetRagdolls(){
	}

	void Die(Vector3 direction){
		//direction = Vector3.Normalize (player.forward);
		Destroy(enemyEmoteCanvas.gameObject);
		transform.Find ("Armature/LowerBody").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature/LowerBody/UpperBody").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature/LowerBody/UpperBody/Head").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature/LowerBody/UpperBody/CollarBone_L/UpperArm_L").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature/LowerBody/UpperBody/CollarBone_L/UpperArm_L/LowerArm_L").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature/LowerBody/UpperBody/CollarBone_R/UpperArm_R").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature/LowerBody/UpperBody/CollarBone_R/UpperArm_R/LowerArm_R").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature/LowerBody/UpperLeg_L").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature/LowerBody/UpperLeg_L/LowerLeg_L").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature/LowerBody/UpperLeg_R").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature/LowerBody/UpperLeg_R/LowerLeg_R").GetComponent<Rigidbody> ().isKinematic = false;
		//transform.Find ("Armature/LowerBody/UpperBody/Head").SetParent = null;
		//transform.Find ("Armature/LowerBody/UpperBody/Head").GetComponent<
		gameObject.layer = 11;
		transform.Find ("SkeletonAimHelp").gameObject.layer = 11;
		//transform.tag = "Inactive";
		transform.Find ("Armature/LowerBody/UpperBody").GetComponent<Rigidbody> ().AddForce (direction * dieForce);
		transform.Find ("Armature/LowerBody/UpperBody/Head").GetComponent<Rigidbody> ().AddForce (direction * dieForce);
		//transform.Find ("Armature").parent = null;
		animator.enabled = false;
		isAlive = false;
		ui.UpdateCombo ();
		//Destroy(transform.Find ("Armature/LowerBody/UpperBody/Head").gameObject);
	}

	void Randomify(){
		defaultCloseDistance = Random.Range ((1f - randomPercent) * defaultCloseDistance, (1f + randomPercent) * defaultCloseDistance);
		defaultSeeDistance = Random.Range ((1f - randomPercent) * defaultSeeDistance, (1f + randomPercent) * defaultSeeDistance);
		defaultViewAngle = Random.Range ((1f - randomPercent) * defaultViewAngle, (1f + randomPercent) * defaultViewAngle);
		//walkAngleMax = Random.Range ((1f - randomPercent) * walkAngleMax, (1f + randomPercent) * walkAngleMax);
		//rotationSpeed = Random.Range ((1f - randomPercent) * rotationSpeed, (1f + randomPercent) * rotationSpeed);
		movementSpeed = Random.Range ((1f - randomPercent) * movementSpeed, (1f + randomPercent) * movementSpeed);
	}

	void Update () {
		if (isAlive) {
			GetPlayerPosition ();
			CheckPlayerSeen ();
			SetAnimations ();
			if (Input.GetKey ("r")) {
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
		//playerLocalAngles = Quaternion.LookRotation (playerLocalPosition);
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
			//enemyEmoteCanvas.SetText ("...");
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
				//transform.GetComponent<Rigidbody> ().AddForce (direction * 2000f);
				Destroy (gameObject, 5f);
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

	void FireBone(){
		Vector3 fireDirection = player.transform.position-transform.position;
		fireDirection = (fireDirection.normalized+transform.forward).normalized;
		Quaternion rot = Quaternion.LookRotation (fireDirection);
		Transform boneProjectile = Instantiate (bone);
		Vector3 bonePosition = new Vector3 (transform.position.x, 3f, transform.position.z);
		boneProjectile.transform.position = bonePosition + 1f*Vector3.Normalize (transform.forward);
		boneProjectile.transform.rotation = Quaternion.Euler (90f, rot.eulerAngles.y, 0);
		boneProjectile.GetComponent<Rigidbody> ().AddForce (fireDirection * shootForce);
		boneProjectile.GetComponent<Bone> ().moveDirection = fireDirection;
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

	void OnCollisionEnter(Collision col){
		if (col.transform.tag == "Player") {
			Vector3 myForward = transform.forward;
			myForward.y = 0f;
			col.transform.GetComponent<PlayerScript> ().HitEnemy (Vector3.Normalize(myForward), 800f);
		}
	}
}
