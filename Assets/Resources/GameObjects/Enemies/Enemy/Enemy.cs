using UnityEngine;
using System.Collections;

/*
The base enemy class for all enemies
*/
public class Enemy : MonoBehaviour
{
    // Static variables for all enemies
    protected static Transform player;
    protected static UI ui;
    protected static string kAngryPhase = "Angry";
    protected static string kIdlePhase = "Idle";
    protected static string kSuspiciousPhase = "Suspicious";
    protected static int kInactiveLayer = 11;

    // Constants
    protected float kAngryDistance = 10f;
    protected float kAttackForSureDistance = 3f;
    protected float kHitRecentlyDuration = 5f;
    protected float kJoltedDuration = 1f;
    protected float kMovementSpeed = 7f;
    protected float kRandomizePercent = 0.2f;
    protected float kRotationSpeed = 3f;
    protected float kJoltedRotationSpeed = 1f;
    protected float kViewDistance = 20f;
    protected float kSuspiciousAfterAngryDuration = 2f;
    protected float kSuspiciousDistance = 5f;
    protected float kSuspiciousDuration = 2f;
    protected int kTotalHealth = 300;
    protected float kTouchedPlayerForce = 800f;
    protected float kViewAngle = 70f;
    protected float kWalkAngleMax = 35f;

    // Instance variables
    protected Animator animator;
    protected EnemyEmoteCanvasScript enemyEmoteCanvas;
    protected int health;
    protected Bar healthBar;
    protected bool isAlive = true;
    protected float lastHitTime;
    protected float lastAngryTime;
    protected float lastJoltedTime;
    protected float lastSuspiciousTime;
    protected string phase;
    protected float playerRelativeDistance;
    protected Vector3 playerRelativePosition;
    protected Quaternion playerRelativeRotation;
    protected float playerRelativeViewAngle;

    protected virtual void Start() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (ui == null) {
            ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        }
        animator = transform.GetComponent<Animator>();
        healthBar = transform.Find("Bar").GetComponent<Bar>();
        enemyEmoteCanvas = transform.Find("EnemyEmoteCanvas").GetComponent<EnemyEmoteCanvasScript>();
        enemyEmoteCanvas.HideImmediate();
        RandomizeConstants();
        health = kTotalHealth;
        phase = kIdlePhase;
    }

    protected virtual void Ragdoll(){
        transform.Find("Armature/LowerBody").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperBody").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperBody/Head").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperBody/CollarBone_L/Shoulder_L/UpperArm_L").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperBody/CollarBone_L/Shoulder_L/UpperArm_L/LowerArm_L").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperBody/CollarBone_R/Shoulder_R/UpperArm_R").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperBody/CollarBone_R/Shoulder_R/UpperArm_R/LowerArm_R").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperLeg_L").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperLeg_L/LowerLeg_L").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperLeg_R").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("Armature/LowerBody/UpperLeg_R/LowerLeg_R").GetComponent<Rigidbody>().isKinematic = false;
    }

    protected virtual void Die(Vector3 direction) {
        Ragdoll();
        transform.Find("AimHelp").gameObject.layer = kInactiveLayer;
        gameObject.layer = kInactiveLayer;
        Destroy(enemyEmoteCanvas.gameObject);
        isAlive = false;
        animator.enabled = false;
        ui.UpdateCombo();
        healthBar.Die();
    }

    private void Update() {
        if (isAlive) {
            UpdatePlayerVariables();
            UpdatePhase();
            UpdateEmote();
            UpdateHealthBar();
            CheckToFire();
        }
    }

    private void FixedUpdate() {
        if (isAlive) {
            MoveAccordingToPhase();
        }
    }

    private void UpdatePhase() {
        if (player.GetComponent<PlayerScript>().lives > 0) {
            bool isPlayerInViewAngleRange = (playerRelativeViewAngle < kViewAngle || playerRelativeViewAngle > 360-kViewAngle);
            if (WasHitRecently() || IsPlayerWithin(kAngryDistance) || (isPlayerInViewAngleRange && IsPlayerWithin(kViewDistance))) {
                phase = kAngryPhase;
                UpdateAngryTime();
            } else if (IsPlayerWithin(kAngryDistance+kSuspiciousDistance) || (isPlayerInViewAngleRange && IsPlayerWithin(kViewDistance+kSuspiciousDistance))) {
                phase = kSuspiciousPhase;
                UpdateSuspiciousTime();
            } else if (IsInFadingSuspicion()){
                phase = kSuspiciousPhase;
            } else {
                phase = kIdlePhase;
            }
        } else {
            phase = kIdlePhase;
        }
    }

    private void MoveAccordingToPhase() {
        if(player.GetComponent<PlayerScript>().lives > 0 && phase == kAngryPhase){
            if (IsJolted()) {
                RotateToPlayer(kJoltedRotationSpeed);
            } else {
                RotateToPlayer(kRotationSpeed);
                bool isPlayerInWalkAngleRange = (playerRelativeViewAngle < kWalkAngleMax || 360f - playerRelativeViewAngle < kWalkAngleMax);
                if (isPlayerInWalkAngleRange) {
                    MoveTowardsPlayer(kMovementSpeed);
                } else {
                    UpdateWalkingAnimation(false);
                }
            }
        } else {
            UpdateWalkingAnimation(false);
        }
    }

    private void MoveTowardsPlayer(float movementSpeed){
        transform.Translate(Vector3.Normalize(playerRelativePosition) * movementSpeed * Time.deltaTime, Space.World);
        UpdateWalkingAnimation(true);
    }

    private void RotateToPlayer(float rotationSpeed){
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRelativeRotation, rotationSpeed * Time.deltaTime);
    }

    public void Hit(int damage, Vector3 direction)
    {
        if (isAlive)
        {
            health -= damage;
            if (health <= 0)
            {
                Die(direction);
                Destroy(gameObject, 5f);
                player.GetComponent<PlayerScript>().BoostEnergy();
            }
            else
            {
                UpdateHealthBar();
                UpdateLastHitTime();
                if (phase != kAngryPhase)
                {
                    animator.SetTrigger("IsJolted");
                    UpdateJoltedTime();
                }
            }
        }
    }

    protected virtual void FireProjectile(){
        //No-op
    }

    private void CheckToFire()
    {
        if (phase == kAngryPhase && !IsJolted())
        {
            float wasHitRecentlyAdd = 0f;
            if (WasHitRecently()) {
                wasHitRecentlyAdd = 40f;
            }
            if (playerRelativeDistance < kAttackForSureDistance)
            {
                animator.SetTrigger("IsAttacking");
            }
            if (playerRelativeDistance < kAngryDistance)
            {
                if (Random.Range(0f, 100f) < (55f + wasHitRecentlyAdd) * Time.deltaTime)
                {
                    animator.SetTrigger("IsAttacking");
                }
            }
            else if (playerRelativeDistance < kViewDistance)
            {
                if (Random.Range(0f, 100f) < (30f + wasHitRecentlyAdd) * Time.deltaTime)
                {
                    animator.SetTrigger("IsAttacking");
                }
            }
            else if (Random.Range(0f, 100f) < (15f + wasHitRecentlyAdd) * Time.deltaTime)
            {
                animator.SetTrigger("IsAttacking");
            }
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Player")
        {
            //Make the force's y zero, so player isn't pushed up or down
            Vector3 myForward = transform.forward;
            myForward.y = 0f;

            //Push the player back for touching the enemy
            col.transform.GetComponent<PlayerScript>().HitEnemy(Vector3.Normalize(myForward), kTouchedPlayerForce);
        }
    }

    private bool IsJolted()
    {
        return IsTimeInDuration(lastJoltedTime, kJoltedDuration);
    }

    private bool IsInFadingSuspicion()
    {
        return IsTimeInDuration(lastSuspiciousTime, kSuspiciousDuration) || IsTimeInDuration(lastAngryTime, kSuspiciousAfterAngryDuration);
    }

    private bool IsPlayerWithin(float distance)
    {
        return playerRelativeDistance <= distance;
    }

    protected bool IsTimeInDuration(float time, float timeDuration)
    {
        return time != 0 && Time.time - time < timeDuration;
    }

    private bool WasHitRecently()
    {
        return IsTimeInDuration(lastHitTime, kHitRecentlyDuration);
    }

    private void UpdateAngryTime()
    {
        lastAngryTime = Time.time;
    }

    private void UpdateEmote()
    {
        if (phase == kSuspiciousPhase)
        {
            enemyEmoteCanvas.Show();
        }
        else
        {
            enemyEmoteCanvas.HideImmediate();
        }
    }

    void UpdateHealthBar()
    {
        if (WasHitRecently())
        {
            if (health <= 0)
            {
                health = 0;
            }
            healthBar.SetValue(health * 1f / kTotalHealth);
            healthBar.Show();
        }
        else
        {
            healthBar.Hide();
        }
    }

    private void UpdateJoltedTime()
    {
        lastJoltedTime = Time.time;
    }

    private void UpdateLastHitTime()
    {
        lastHitTime = Time.time;
    }

    private void UpdatePlayerVariables()
    {
        Vector3 playerPosition = player.position;
        playerRelativePosition = playerPosition - transform.position;
        playerRelativeDistance = Vector3.Magnitude(playerRelativePosition);
        playerRelativeRotation = Quaternion.Euler(0f, Quaternion.LookRotation(playerRelativePosition).eulerAngles.y, 0f);
        playerRelativeViewAngle = Quaternion.LookRotation(transform.InverseTransformPoint(playerPosition)).eulerAngles.y;
    }

    private void UpdateSuspiciousTime()
    {
        lastSuspiciousTime = Time.time;
    }

    private void UpdateWalkingAnimation(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }

    protected float Randomize(float value)
    {
        return Random.Range((1f - kRandomizePercent), (1f + kRandomizePercent)) * value;
    }

    protected virtual void RandomizeConstants()
    {
        kHitRecentlyDuration = Randomize(kHitRecentlyDuration);
        kAngryDistance = Randomize(kAngryDistance);
        kViewDistance = Randomize(kViewDistance);
        kViewAngle = Randomize(kViewAngle);
        kMovementSpeed = Randomize(kMovementSpeed);
    }
}