using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Enemy {
    private float kDieUpperBodyForce = 500f;
    private static float kDieDisappearTime = 1.2f;

    private static Transform kBat;
    private static string kBatPath = "GameObjects/Bat/Bat";
    private static float kBatXRotation = 10f;
    private static float kBatYPosition = 3f;

    private static Transform kVampireBat;
    private static string kVampireBatPath = "GameObjects/VampireBat/VampireBat";
    private static float kVampireBatXRotation = -5f;
    private static int kVampireTotalHealth = 3000;

    protected override void Start() {
        kTotalHealth = kVampireTotalHealth;
        base.Start();
        if (kBat == null) {
            kBat = ((GameObject)Resources.Load(kBatPath, typeof(GameObject))).transform;
        }
        if (kVampireBat == null) {
            kVampireBat = ((GameObject)Resources.Load(kVampireBatPath, typeof(GameObject))).transform;
        }
    }

    protected override void Die(Vector3 direction) {
        base.Die(direction);
        transform.Find("Armature/LowerBody/UpperBody").GetComponent<Rigidbody>().AddForce(direction * kDieUpperBodyForce);
        Destroy(gameObject, kDieDisappearTime);
        Invoke ("SpawnVampireBat", kDieDisappearTime-0.01f);
    }

    private void SpawnVampireBat() {
        Transform vampireBat = Instantiate (kVampireBat);
        Transform head = transform.Find ("Armature/LowerBody/UpperBody/Head");
        vampireBat.position = head.position;
        vampireBat.rotation = Quaternion.Euler (kVampireBatXRotation, head.rotation.eulerAngles.y, 0f);
    }

    protected override void RandomizeConstants() {
        base.RandomizeConstants();
        kDieUpperBodyForce = Randomize(kDieUpperBodyForce);
    }

    protected override void FireProjectile() {
        Transform bat = Instantiate(kBat);
        bat.position = new Vector3(transform.position.x, kBatYPosition, transform.position.z) + Vector3.Normalize(transform.forward);
        bat.rotation = Quaternion.Euler(kBatXRotation, transform.rotation.eulerAngles.y, 0f);
    }
}