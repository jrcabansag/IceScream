using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy {
    private float kDieHeadForce = 300f;
    private float kDieUpperBodyForce = 4000f;
    private static Transform kProjectile;
    private static string kProjectilePath = "GameObjects/Bone/Bone";
    private float kProjectileShootForce = 1500f;
    private static float kProjectileYPosition = 3f;

    protected override void Start(){
        base.Start();
        if (kProjectile == null){
            kProjectile = ((GameObject)Resources.Load(kProjectilePath, typeof(GameObject))).transform;
        }
    }

    protected override void Die(Vector3 direction){
        base.Die(direction);
        transform.Find ("Armature/LowerBody/UpperBody").GetComponent<Rigidbody> ().AddForce (direction * kDieUpperBodyForce);
		transform.Find ("Armature/LowerBody/UpperBody/Head").GetComponent<Rigidbody> ().AddForce (direction * kDieHeadForce);
    }

    protected override void RandomizeConstants() {
        base.RandomizeConstants();
        kDieHeadForce = Randomize(kDieHeadForce);
        kDieUpperBodyForce = Randomize(kDieUpperBodyForce);
    }

    protected override void FireProjectile() {
        Vector3 projectileDirection = (playerRelativePosition.normalized + transform.forward).normalized;
        float projectileRotation = Quaternion.LookRotation(projectileDirection).eulerAngles.y;
        Vector3 projectilePosition = new Vector3(transform.position.x, kProjectileYPosition, transform.position.z);

        Transform projectile = Instantiate(kProjectile);
        projectile.transform.position = projectilePosition+Vector3.Normalize(transform.forward);
        projectile.transform.rotation = Quaternion.Euler(90f, projectileRotation, 0);
        projectile.GetComponent<Rigidbody>().AddForce(projectileDirection * kProjectileShootForce);
        projectile.GetComponent<Bone>().moveDirection = projectileDirection;
    }

    protected override void Ragdoll() {
        transform.Find("Armature/LowerBody").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperBody").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperBody/Head").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperBody/CollarBone_L/UpperArm_L").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperBody/CollarBone_L/UpperArm_L/LowerArm_L").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperBody/CollarBone_R/UpperArm_R").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperBody/CollarBone_R/UpperArm_R/LowerArm_R").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperLeg_L").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperLeg_L/LowerLeg_L").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperLeg_R").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperLeg_R/LowerLeg_R").GetComponent<Rigidbody>().isKinematic = false;
    }
}
