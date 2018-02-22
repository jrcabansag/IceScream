using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public float defaultMovementSpeed = 6f;
	public float defaultRotationSpeed = 5f;
	public float defaultAnimationSpeed = 1.5f;
	public float defaultFireDifference = 0.7f;
	public float slideMovementMultiplier = 2.35f;
	public float slideRotationMultiplier = 2f;
	public float slideAnimationMultiplier = 1.7f;
	public float slideFireDifference = 0.4f;
	public float slideFireTimeDifference = 1f;
	public float slidePuddleSpawnTime = 0.01f;
	public float totalEnergy = 100f;
	public float energyDepletion = 10f;
	public float energyGeneration = 10f;
	public float minimumSlideEnergy = 20f;
	public float energyBoost = 30f;
	public float invincibleTime = 2f;
	public float showHeartTime = 5f;
	public float shootForce = 1000f;
	private Transform iceCream;
	private Transform iceCreamPuddle;
	private Transform mainCamera;
	private Bar energyBar;
	private Hearts hearts;
	private Animator animator;
	private Vector3 mousePosition;
	private float verticalInput;
	private float horizontalInput;
	private float lastFireTime;
	private float lastEarlyFireTime;
	private float lastSlideTime;
	private float lastSlidePuddleSpawnTime;
	private float lastHitTime;
	private float currentEnergy = 100f;
	private int lives = 3;
	private bool isSliding = false;
	private bool isAttacking = false;
	private bool onRamp = false;
	private bool isAlive = true;

	void Start () {
		animator = transform.GetComponent<Animator> ();
		lastFireTime = Time.time;
		iceCream = ((GameObject)Resources.Load("GameObjects/IceCream/IceCream", typeof(GameObject))).transform;
		iceCreamPuddle = ((GameObject)Resources.Load("GameObjects/IceCreamPuddle/IceCreamPuddle", typeof(GameObject))).transform;
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").transform;
		energyBar = transform.Find ("Bar").GetComponent<Bar>();
		hearts = transform.Find ("Hearts").GetComponent<Hearts>();
	}

	void Update () {
		if (isAlive) {
			GetInputs ();
			UpdateEnergy ();
			SetAnimations ();
			SpawnIceCreamPuddle ();
			UpdateHearts ();
		}
	}

	void FixedUpdate(){
		if (isAlive) {
			MovePlayer ();
			RotatePlayer ();
		}
	}

	Vector3 GetMousePosition(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit[] hits = Physics.RaycastAll (ray, 100f);
		foreach (RaycastHit hit in hits) {
			string hitTag = hit.transform.tag;
			if (hitTag == "Enemy" && hit.transform.gameObject.layer == 12) {
				return new Vector3 (hit.transform.position.x, transform.position.y, hit.transform.position.z);
			}
		}
		foreach (RaycastHit hit in hits) {
			string hitTag = hit.transform.tag;
			if (hitTag == "Obstacle") {
				return new Vector3 (hit.point.x, transform.position.y, hit.point.z);
			}
		}
		foreach (RaycastHit hit in hits) {
			string hitTag = hit.transform.tag;
			if (hitTag == "Ground") {
				return new Vector3 (hit.point.x, transform.position.y, hit.point.z);
			}
		}
		return transform.position + transform.forward;
	}

	void GetInputs(){
		mousePosition = GetMousePosition ();
		verticalInput = Input.GetAxis ("Vertical");
		horizontalInput = Input.GetAxis ("Horizontal");
		//if (Input.GetKey ("space") && (Mathf.Abs(verticalInput) != 0 || Mathf.Abs(horizontalInput) != 0)) {
		if (Input.GetKey ("space") && !isSliding && currentEnergy >= minimumSlideEnergy) {
			isSliding = true;
			lastSlideTime = Time.time;
		}
		else if (Input.GetKey ("space") && isSliding && currentEnergy > 0) {
			isSliding = true;
			lastSlideTime = Time.time;
		} else {
			isSliding = false;
		}
		if (Input.GetMouseButton (0)) {
			isAttacking = true;
		} else {
			isAttacking = false;
		}
	}

	void SetAnimations(){
		if (isSliding) {
			animator.speed = defaultAnimationSpeed * slideAnimationMultiplier;
		} else {
			animator.speed = defaultAnimationSpeed;
		}
		animator.SetBool ("IsSliding", isSliding);
		if (isAttacking) {
			animator.SetBool ("IsAttacking", true);
		} else {
			animator.SetBool ("IsAttacking", false);
		}
		if (Mathf.Abs(verticalInput) == 1 || Mathf.Abs(horizontalInput) == 1) {
			animator.SetBool ("IsMoving", true);
			Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
			if (Vector3.Angle (movement, transform.forward) > 90) {
				animator.SetBool ("IsBackward", true);
			} else {
				animator.SetBool ("IsBackward", false);
			}
		} else {
			animator.SetBool ("IsMoving", false);
		}
	}

	void MovePlayer(){
		float movementSpeed = defaultMovementSpeed;
		if ((verticalInput == -1 || verticalInput == 1) && (horizontalInput == -1 || horizontalInput == 1)) {
			movementSpeed /= Mathf.Sqrt (2f);
		}
		if (isSliding) {
			movementSpeed *= slideMovementMultiplier;
		}
		Vector3 upDirection = new Vector3(mainCamera.forward.x, 0f, mainCamera.forward.z);
		upDirection = Vector3.Normalize(upDirection);
		Vector3 rightDirection = Quaternion.Euler(0, 90f, 0) * upDirection;
		rightDirection = Vector3.Normalize (rightDirection);
		Vector3 movement = movementSpeed * Time.deltaTime * upDirection * verticalInput + movementSpeed * Time.deltaTime * rightDirection * horizontalInput;
		//Vector3 movement = new Vector3 (movementSpeed * Time.deltaTime * horizontalInput, 0f, movementSpeed * Time.deltaTime * verticalInput);
		transform.Translate (movement, Space.World);
	}

	void RotatePlayer(){
		float rotationSpeed = defaultRotationSpeed;
		if (isSliding) {
			rotationSpeed *= slideRotationMultiplier;
		}
		Quaternion rotationAngle = Quaternion.Euler(0f, Quaternion.LookRotation(mousePosition - transform.position).eulerAngles.y, 0f);
		//rotationAngle = Quaternion.Euler(0f, rotationAngle.)
		transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, rotationSpeed * Time.deltaTime);
	}

	void Fire(){
		float currentTime = Time.time;
		float deltaTime = currentTime - lastFireTime;
		float deltaSlideTime = currentTime - lastSlideTime;
		float fireDifference = defaultFireDifference;
		if (isSliding || deltaSlideTime < slideFireTimeDifference) {
			fireDifference = slideFireDifference;
		}
		if (deltaTime >= fireDifference) {
			lastFireTime = currentTime;
			Vector3 fireDirection = transform.forward;
			if (Input.GetMouseButton (0)) {
				fireDirection = GetMousePosition () - transform.position;
			}
			fireDirection = fireDirection.normalized;
			Quaternion rot = Quaternion.LookRotation (fireDirection);
			Transform iceCreamProjectile = Instantiate (iceCream);
			Vector3 iceCreamPosition = new Vector3 (transform.position.x, 3f, transform.position.z);
			iceCreamProjectile.transform.position = iceCreamPosition + 1f * Vector3.Normalize (transform.forward);
			iceCreamProjectile.transform.rotation = Quaternion.Euler (90f, rot.eulerAngles.y, 0);
			iceCreamProjectile.GetComponent<Rigidbody> ().AddForce (fireDirection * shootForce);
		} else if (currentTime - lastEarlyFireTime >= fireDifference*4f) {
			lastEarlyFireTime = currentTime;
			lastFireTime = currentTime;
			Vector3 fireDirection = transform.forward;
			if (Input.GetMouseButton (0)) {
				fireDirection = GetMousePosition () - transform.position;
			}
			fireDirection = fireDirection.normalized;
			Quaternion rot = Quaternion.LookRotation (fireDirection);
			Transform iceCreamProjectile = Instantiate (iceCream);
			Vector3 iceCreamPosition = new Vector3 (transform.position.x, 3f, transform.position.z);
			iceCreamProjectile.transform.position = iceCreamPosition + 1f * Vector3.Normalize (transform.forward);
			iceCreamProjectile.transform.rotation = Quaternion.Euler (90f, rot.eulerAngles.y, 0);
			iceCreamProjectile.GetComponent<Rigidbody> ().AddForce (fireDirection * shootForce);
		}
	}

	void SpawnIceCreamPuddle(){
		if (isSliding && (lastSlidePuddleSpawnTime == 0 || Time.time-lastSlidePuddleSpawnTime > slidePuddleSpawnTime)) {
			Transform myIceCreamPuddle = Instantiate (iceCreamPuddle);
			Vector3 puddlePosition = transform.position;
			puddlePosition.y -= 0f;
			myIceCreamPuddle.transform.position = puddlePosition;
			if (onRamp) {
				myIceCreamPuddle.transform.rotation = Quaternion.Euler (60f, 0f, 0f);
			}
			lastSlidePuddleSpawnTime = Time.time;
		}
	}

	void UpdateEnergy(){
		if (isSliding) {
			currentEnergy -= energyDepletion * Time.deltaTime;
		} else {
			currentEnergy += energyGeneration * Time.deltaTime;
		}
		if (currentEnergy < 0) {
			currentEnergy = 0f;
		} else if (currentEnergy > totalEnergy) {
			currentEnergy = totalEnergy;
		}
		energyBar.SetValue(currentEnergy/totalEnergy);
	}

	public void BoostEnergy(){
		currentEnergy += energyBoost;
		if (currentEnergy > totalEnergy) {
			currentEnergy = totalEnergy;
		}
	}

	public void WasHit(Vector3 direction){
		if (lastHitTime == 0 || Time.time - lastHitTime > invincibleTime) {
			print ("WAS HIT!");
			hearts.Show ();
			hearts.DropLife ();
			lastHitTime = Time.time;
			lives -= 1;
		}
		print ("Lives is " + lives);
		if (lives == 0) {
			Die (direction);
		}
	}

	void UpdateHearts(){
		if (Time.time - lastHitTime < showHeartTime) {
			hearts.Show ();
		} else {
			hearts.Hide ();
		}
	}

	void Die(Vector3 direction){
		transform.Find ("Armature_001/LowerBody").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperBody").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperBody/Head").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperBody/CollarBone_L/Shoulder_L/UpperArm_L").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperBody/CollarBone_L/Shoulder_L/UpperArm_L/LowerArm_L").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperBody/CollarBone_R/Shoulder_R/UpperArm_R").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperBody/CollarBone_R/Shoulder_R/UpperArm_R/LowerArm_R").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperLeg_L").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperLeg_L/LowerLeg_L").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperLeg_R").GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Armature_001/LowerBody/UpperLeg_R/LowerLeg_R").GetComponent<Rigidbody> ().isKinematic = false;
		Destroy (transform.Find ("Bar").gameObject);
		Destroy (transform.Find ("Hearts").gameObject);
		transform.Find ("Armature_001/LowerBody/UpperBody/Head").GetComponent<Rigidbody> ().AddForce (direction * 500f);
		animator.enabled = false;
		gameObject.layer = 11;
		isAlive = false;
	}
}
