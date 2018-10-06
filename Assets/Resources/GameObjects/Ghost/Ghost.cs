using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy {
    private float kDieUpperBodyForce = 500f;
    private static float kDieSinkTime = 2.5f;

    private static Transform kSoul;
    private static int kSoulCount = 8;
    private float kSoulForce = 1500f;
    private static string kSoulPath = "GameObjects/Soul/Soul";
    private static float kSoulYPosition = 3f;
    private static float kSoulInitialDistance = 3f;

    private static int kSinkLayer = 23;
    private static float kSinkDrag = 30f;
    private static float kSinkDragSlow = 6f;

    private static int kGhostTotalHealth = 800;

    protected override void Start() {
        kTotalHealth = kGhostTotalHealth;
        if (kSoul == null) {
            kSoul = ((GameObject)Resources.Load(kSoulPath, typeof(GameObject))).transform;
        }
        base.Start();
    }

    protected override void Die(Vector3 direction) {
        base.Die(direction);
        transform.Find("Armature/LowerBody/UpperBody").GetComponent<Rigidbody>().AddForce(direction * kDieUpperBodyForce);
        Invoke("Sink", kDieSinkTime);
    }

    protected override void RandomizeConstants() {
        base.RandomizeConstants();
        kDieUpperBodyForce = Randomize(kDieUpperBodyForce);
    }

    protected override void FireProjectile() {
        for (int soulIndex = 0; soulIndex < kSoulCount; soulIndex++) {
            Vector3 soulDirection = Quaternion.Euler(0f, ((float)360 / kSoulCount * soulIndex), 0) * transform.forward;
            FireSoul(Vector3.Normalize(soulDirection));
        }
    }

    private void FireSoul(Vector3 soulDirection) {
        Transform soul = Instantiate (kSoul);
        soul.GetComponent<Soul> ().ghostAnchor = transform;
        soul.rotation = Quaternion.LookRotation(soulDirection);
        Vector3 soulPosition = new Vector3 (transform.position.x, kSoulYPosition, transform.position.z);
        if (isWalking) {
            soulPosition = soulPosition + kSoulInitialDistance * Vector3.Normalize (transform.forward);
        }
        soul.transform.position = soulPosition;
        soul.GetComponent<Rigidbody> ().AddForce (soulDirection * kSoulForce);
        soul.GetComponent<Soul> ().moveDirection = soulDirection;
        soul.GetComponent<Soul> ().activated = true;
    }

    private void Sink() {
        transform.Find ("Armature/LowerBody").gameObject.layer = kSinkLayer;
        transform.Find ("Armature/LowerBody/UpperBody").gameObject.layer = kSinkLayer;
        transform.Find ("Armature/LowerBody/UpperBody/Head").gameObject.layer = kSinkLayer;
        transform.Find ("Armature/LowerBody/UpperBody/CollarBone_L/Shoulder_L/UpperArm_L").gameObject.layer = kSinkLayer;
        transform.Find ("Armature/LowerBody/UpperBody/CollarBone_L/Shoulder_L/UpperArm_L/LowerArm_L").gameObject.layer = kSinkLayer;
        transform.Find ("Armature/LowerBody/UpperBody/CollarBone_R/Shoulder_R/UpperArm_R").gameObject.layer = kSinkLayer;
        transform.Find ("Armature/LowerBody/UpperBody/CollarBone_R/Shoulder_R/UpperArm_R/LowerArm_R").gameObject.layer = kSinkLayer;
        transform.Find ("Armature/LowerBody/UpperLeg_L").gameObject.layer = kSinkLayer;
        transform.Find ("Armature/LowerBody/UpperLeg_L/LowerLeg_L").gameObject.layer = kSinkLayer;
        transform.Find ("Armature/LowerBody/UpperLeg_R").gameObject.layer = kSinkLayer;
        transform.Find ("Armature/LowerBody/UpperLeg_R/LowerLeg_R").gameObject.layer = kSinkLayer;
        transform.Find ("Armature/LowerBody").GetComponent<Rigidbody>().drag = kSinkDrag;
        transform.Find("Armature/LowerBody/UpperBody").GetComponent<Rigidbody>().drag = kSinkDragSlow;
        transform.Find ("Armature/LowerBody/UpperBody/Head").GetComponent<Rigidbody>().drag = kSinkDragSlow;
        transform.Find ("Armatur/LowerBody/UpperBody/CollarBone_L/Shoulder_L/UpperArm_L").GetComponent<Rigidbody>().drag = kSinkDrag;
        transform.Find ("Armature/LowerBody/UpperBody/CollarBone_L/Shoulder_L/UpperArm_L/LowerArm_L").GetComponent<Rigidbody>().drag = kSinkDrag;
        transform.Find ("Armature/LowerBody/UpperBody/CollarBone_R/Shoulder_R/UpperArm_R").GetComponent<Rigidbody>().drag = kSinkDrag;
        transform.Find ("Armature/LowerBody/UpperBody/CollarBone_R/Shoulder_R/UpperArm_R/LowerArm_R").GetComponent<Rigidbody>().drag = kSinkDrag;
        transform.Find ("Armature/LowerBody/UpperLeg_L").GetComponent<Rigidbody>().drag = kSinkDrag;
        transform.Find ("Armature/LowerBody/UpperLeg_L/LowerLeg_L").GetComponent<Rigidbody>().drag = kSinkDrag;
        transform.Find ("Armature/LowerBody/UpperLeg_R").GetComponent<Rigidbody>().drag = kSinkDrag;
        transform.Find ("Armature/LowerBody/UpperLeg_R/LowerLeg_R").GetComponent<Rigidbody>().drag = kSinkDrag;
    }
}