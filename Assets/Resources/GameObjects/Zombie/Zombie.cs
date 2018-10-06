using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy {
    private float kDieHeadForce = 500f;
    private float kDieUpperBodyForce = 500f;
    private static Transform kProjectile;
    private static string kProjectilePath = "GameObjects/Brain/Brain";
    private float kProjectileShootForce = 700f;
    private float kProjectileRotation = 25f;
    private static float kProjectileYPosition = 3f;
    private static int kZombieTotalHealth = 500;

    protected override void Start() {
        kTotalHealth = kZombieTotalHealth;
        base.Start();
        if (kProjectile == null) {
            kProjectile = ((GameObject)Resources.Load(kProjectilePath, typeof(GameObject))).transform;
        }
    }

    protected override void Die(Vector3 direction) {
        base.Die(direction);
        transform.Find("Armature/LowerBody/UpperBody").GetComponent<Rigidbody>().AddForce(direction * kDieUpperBodyForce);
        transform.Find("Armature/LowerBody/UpperBody/Head").GetComponent<Rigidbody>().AddForce(direction * kDieHeadForce);
    }

    protected override void RandomizeConstants() {
        base.RandomizeConstants();
        kDieHeadForce = Randomize(kDieHeadForce);
        kDieUpperBodyForce = Randomize(kDieUpperBodyForce);
    }

    protected override void FireProjectile() {
        Vector3 projectileDirection = (playerRelativePosition.normalized+transform.forward).normalized;
        FireSingleProjectile(projectileDirection);
        FireSingleProjectile(Quaternion.Euler(0f, kProjectileRotation, 0)*projectileDirection);
        FireSingleProjectile(Quaternion.Euler(0f, -kProjectileRotation, 0) * projectileDirection);
    }

    private void FireSingleProjectile(Vector3 projectileDirection) {
        Transform projectile = Instantiate(kProjectile);
        float projectileRotation = Quaternion.LookRotation(projectileDirection).eulerAngles.y;
        Vector3 projectilePosition = new Vector3(transform.position.x, kProjectileYPosition, transform.position.z);
        projectile.transform.position = projectilePosition + Vector3.Normalize(transform.forward);
        projectile.transform.rotation = Quaternion.Euler(90f, projectileRotation, 0);
        projectile.GetComponent<Rigidbody>().AddForce(projectileDirection * kProjectileShootForce);
        projectile.GetComponent<Brain>().moveDirection = projectileDirection;

    }
}
