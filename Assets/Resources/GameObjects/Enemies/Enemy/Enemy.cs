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

    // Constants
    protected string kAngryPhase = "Angry";
    protected float kAngryDistance = 10f;
    protected float kAttackForSureDistance = 3f;
    protected float kHitRecentlyDuration = 5f;
    protected string kIdlePhase = "Idle";
    protected float kJoltedDuration = 1f;
    protected float kMovementSpeed = 7f;
    protected float kRandomizePercent = 0.2f;
    protected float kRotationSpeed = 3f;
    protected float kJoltedRotationSpeed = 1f;
    protected float kViewDistance = 20f;
    protected float kSuspiciousAfterAngryDuration = 2f;
    protected float kSuspiciousDistance = 5f;
    protected float kSuspiciousDuration = 2f;
    protected string kSuspiciousPhase = "Suspicious";
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

    protected virtual void Die(Vector3 direction) {
        Destroy(enemyEmoteCanvas.gameObject);
        isAlive = false;
        gameObject.layer = 11;
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
            ShouldFire();
        }
    }

    private void FixedUpdate() {
        if (isAlive) {
            MoveAccordingToPhase();
        }
    }

    private void UpdatePlayerVariables() {
        Vector3 playerPosition = player.position;
        playerRelativePosition = playerPosition - transform.position;
        playerRelativeDistance = Vector3.Magnitude(playerRelativePosition);
        playerRelativeRotation = Quaternion.Euler(0f, Quaternion.LookRotation(playerRelativePosition).eulerAngles.y, 0f);
        playerRelativeViewAngle = Quaternion.LookRotation(transform.InverseTransformPoint(playerPosition)).eulerAngles.y;
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
        if(player.GetComponent<PlayerScript>().lives > 0 && phase == "Angry"){
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

    private void UpdateLastHitTime() {
        lastHitTime = Time.time;
    }

    private void UpdateSuspiciousTime() {
        lastSuspiciousTime = Time.time;
    }

    private void UpdateJoltedTime() {
        lastJoltedTime = Time.time;
    }

    private void UpdateAngryTime() {
        lastAngryTime = Time.time;
    }

    private bool IsPlayerWithin(float distance)
    {
        return playerRelativeDistance <= distance;
    }

    private bool IsJolted()
    {
        return IsTimeInDuration(lastJoltedTime, kJoltedDuration);
    }

    private bool IsInFadingSuspicion()
    {
        return IsTimeInDuration(lastSuspiciousTime, kSuspiciousDuration) || IsTimeInDuration(lastAngryTime, kSuspiciousAfterAngryDuration);
    }

    private bool WasHitRecently()
    {
        return IsTimeInDuration(lastHitTime, kHitRecentlyDuration);
    }

    private void UpdateWalkingAnimation(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }

    private void UpdateEmote()
    {
        if (phase == kSuspiciousPhase) {
            enemyEmoteCanvas.Show();
        } else {
            enemyEmoteCanvas.HideImmediate();
        }
    }

    public void WasHit(int damage, Vector3 direction)
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
                UpdateHealth();
                UpdateLastHitTime();
                if (phase != "Angry")
                {
                    animator.SetTrigger("IsHit");
                    UpdateJoltedTime();
                }
            }
        }
    }

    void ShouldFire()
    {
        if (phase == "Angry")
        {
            float wasHitRecentlyAdd = 0f;
            if (WasHitRecently()) {
                wasHitRecentlyAdd = 40f;
            }
            if (playerRelativeDistance < kAttackForSureDistance)
            {
                animator.SetTrigger("IsAttacking");
            }
            if (playerRelativeDistance < kAngryDistance / 2)
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

    void UpdateHealth()
    {
        if (health <= 0)
        {
            health = 0;
        }
        healthBar.SetValue(health * 1f / kTotalHealth);
    }

    void UpdateHealthBar()
    {
        if (WasHitRecently()) {
            healthBar.Show();
        } else {
            healthBar.Hide();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Player")
        {
            //Keep pushing force y zero, so player isn't pushed up or down
            Vector3 myForward = transform.forward;
            myForward.y = 0f;

            //Push the player
            col.transform.GetComponent<PlayerScript>().HitEnemy(Vector3.Normalize(myForward), kTouchedPlayerForce);
        }
    }

    bool IsTimeInDuration(float time, float timeDuration)
    {
        return time != 0 && Time.time - time < timeDuration;
    }

    protected float Randomize(float value){
        return Random.Range((1f - kRandomizePercent), (1f + kRandomizePercent))*value;
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