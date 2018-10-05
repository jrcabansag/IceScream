using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonScript : Enemy {
    private static Transform bone;


    protected override void Start(){
        base.Start();
        if (bone == null){
            bone = ((GameObject)Resources.Load("GameObjects/Bone/Bone", typeof(GameObject))).transform;
        }
    }

    protected override void Die(Vector3 direction){
        base.Die(direction);
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
		transform.Find ("Armature/LowerBody/UpperBody").GetComponent<Rigidbody> ().AddForce (direction * kDieForce);
		transform.Find ("Armature/LowerBody/UpperBody/Head").GetComponent<Rigidbody> ().AddForce (direction * kDieForce);
        transform.Find("SkeletonAimHelp").gameObject.layer = 11;
    }


	void FireBone(){
		Vector3 fireDirection = player.transform.position-transform.position;
		fireDirection = (fireDirection.normalized+transform.forward).normalized;
		Quaternion rot = Quaternion.LookRotation (fireDirection);
		Transform boneProjectile = Instantiate (bone);
		Vector3 bonePosition = new Vector3 (transform.position.x, 3f, transform.position.z);
		boneProjectile.transform.position = bonePosition + 1f*Vector3.Normalize (transform.forward);
		boneProjectile.transform.rotation = Quaternion.Euler (90f, rot.eulerAngles.y, 0);
		boneProjectile.GetComponent<Rigidbody> ().AddForce (fireDirection * kShootForce);
		boneProjectile.GetComponent<Bone> ().moveDirection = fireDirection;
	}
}
