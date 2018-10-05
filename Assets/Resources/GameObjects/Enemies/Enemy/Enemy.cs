using UnityEngine;
using System.Collections;

/*
The base enemy class for all enemies
*/
public class Enemy : MonoBehaviour
{
    protected static Transform player;
    protected static UI ui;

    protected Animator animator;
    protected Bar healthBar;
    protected EnemyEmoteCanvasScript enemyEmoteCanvas;

    protected float kRandomizePercent = 0.2f;
    protected float kCloseDistance = 10f;
    protected float kSeeDistance = 20f;
    protected float kViewAngle = 70f;
    protected float kWillShootDistance = 2f;
    protected float kMovementSpeed = 7f;
    protected float kWalkAngleMax = 35f;
    protected float kRotationSpeed = 3f;
    protected float kAngryDistanceMultiplier = 2.5f;
    protected float kAngryDuration = 5f;
    protected float kSuspiciousDistance = 5f;
    protected float kSuspiciousDuration = 2f;
    protected float kSuspiciousAfterAwareDuration = 2f;
    protected float kShootForce = 1500f;
    protected float kJoltedDuration = 1f;
    protected float kDieForce = 2000f;
    protected int kTotalHealth = 300;

    protected int currentHealth;
    protected float lastAngryTime;
    protected float lastSuspiciousTime;
    protected float lastJoltedTime;
    protected float lastAwareTime;
    protected bool isAlive = true;
    private string phase = "Idle";
    private bool isWalking = false;


    private Vector3 playerPosition;
    private Vector3 playerLocalPosition;
    private Quaternion playerLocalAngles;
    private Vector3 playerViewAngles;
    private float playerDistance;

    protected virtual void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (ui == null)
        {
            ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        }
        animator = transform.GetComponent<Animator>();
        healthBar = transform.Find("Bar").GetComponent<Bar>();
        enemyEmoteCanvas = transform.Find("EnemyEmoteCanvas").GetComponent<EnemyEmoteCanvasScript>();
        enemyEmoteCanvas.HideImmediate();
        RandomizeConstants();
        currentHealth = kTotalHealth;
    }

    protected virtual void Update()
    {
        if (isAlive)
        {
            GetPlayerPosition();
            CheckPlayerSeen();
            SetAnimations();
            if (Input.GetKey("r") && phase == "Aware")
            {
                animator.SetTrigger("IsAttacking");
            }
            SetEmote();
            ShouldFire();
            SetHealthBar();
        }
    }

    protected virtual void Die(Vector3 direction){
        Destroy(enemyEmoteCanvas.gameObject);
        isAlive = false;
        gameObject.layer = 11;
        animator.enabled = false;
        ui.UpdateCombo();
    }

    void FixedUpdate()
    {
        if (isAlive)
        {
            MoveTowardsPlayer();
        }
    }


    void GetPlayerPosition()
    {
        playerPosition = player.transform.position;
        playerLocalPosition = playerPosition - transform.position;
        playerDistance = Vector3.Magnitude(playerLocalPosition);
        //playerLocalAngles = Quaternion.LookRotation (playerLocalPosition);
        playerLocalAngles = Quaternion.Euler(0f, Quaternion.LookRotation(playerLocalPosition).eulerAngles.y, 0f);
        playerViewAngles = Quaternion.LookRotation(transform.InverseTransformPoint(playerPosition)).eulerAngles;
    }

    void CheckPlayerSeen()
    {
        if (player.GetComponent<PlayerScript>().lives > 0)
        {
            float closeDistance = kCloseDistance;
            float seeDistance = kSeeDistance;
            float searchingDistance = kSeeDistance;
            float viewAngle = kViewAngle;
            if (lastAngryTime != 0 && Time.time - lastAngryTime < kAngryDuration)
            {
                viewAngle = 360f;
                searchingDistance = 1000f;
            }
            if (playerDistance < closeDistance)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, playerLocalAngles, kRotationSpeed * Time.deltaTime);
                phase = "Aware";
            }
            else if (((playerViewAngles.y < viewAngle || playerViewAngles.y > 360 - viewAngle) && playerDistance < seeDistance))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, playerLocalAngles, kRotationSpeed * Time.deltaTime);
                phase = "Aware";
            }
            else if (((playerViewAngles.y < viewAngle || playerViewAngles.y > 360 - viewAngle) && playerDistance < searchingDistance))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, playerLocalAngles, 1f * Time.deltaTime);
                phase = "Searching";
            }
            else if (playerDistance < closeDistance + kSuspiciousDistance || ((playerViewAngles.y < viewAngle || playerViewAngles.y > 360 - viewAngle) && playerDistance < seeDistance + kSuspiciousDistance))
            {
                if (!IsStillAware())
                {
                    phase = "Suspicious";
                    MakeSuspicious();
                }
            }
            else
            {
                phase = "Idle";
            }
        }
        else
        {
            phase = "Idle";
        }
    }

    void MoveTowardsPlayer()
    {
        if (player.GetComponent<PlayerScript>().lives > 0)
        {
            if (phase == "Aware" && (playerViewAngles.y < kWalkAngleMax || 360f - playerViewAngles.y < kWalkAngleMax))
            {
                transform.Translate(Vector3.Normalize(playerLocalPosition) * kMovementSpeed * Time.deltaTime, Space.World);
                isWalking = true;
                MakeStillAware();
            }
            else if (phase == "Searching" && (playerViewAngles.y < kWalkAngleMax || 360f - playerViewAngles.y < kWalkAngleMax) && CheckIfPlayerInSight())
            {
                if (Time.time - lastJoltedTime > kJoltedDuration)
                {
                    transform.Translate(Vector3.Normalize(playerLocalPosition) * kMovementSpeed * Time.deltaTime, Space.World);
                    isWalking = true;
                    MakeStillAware();
                }
            }
            else
            {
                isWalking = false;
            }
        }
        else
        {
            isWalking = false;
        }
    }

    void SetAnimations()
    {
        animator.SetBool("IsWalking", isWalking);
    }

    public void MakeAngry()
    {
        lastAngryTime = Time.time;
    }

    public void MakeSuspicious()
    {
        lastSuspiciousTime = Time.time;
    }

    public void MakeJolted()
    {
        lastJoltedTime = Time.time;
    }

    public void MakeStillAware()
    {
        lastAwareTime = Time.time;
    }

    bool IsStillAware()
    {
        return lastAwareTime != 0f && Time.time - lastAwareTime < kSuspiciousAfterAwareDuration;
    }

    bool IsAngry()
    {
        return lastAngryTime != 0 && Time.time - lastAngryTime < kAngryDuration;
    }

    void SetEmote()
    {
        if (phase == "Aware" || phase == "Searching")
        {
            enemyEmoteCanvas.HideImmediate();
        }
        else if (lastSuspiciousTime != 0f && Time.time - lastSuspiciousTime < kSuspiciousDuration)
        {
            enemyEmoteCanvas.Show();
        }
        else if (lastAwareTime != 0f && Time.time - lastAwareTime < kSuspiciousAfterAwareDuration)
        {
            //enemyEmoteCanvas.SetText ("...");
        }
        else
        {
            enemyEmoteCanvas.HideImmediate();
        }
    }



    public void WasHit(int damage, Vector3 direction)
    {
        if (isAlive)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die(direction);
                player.GetComponent<PlayerScript>().BoostEnergy();
                //transform.GetComponent<Rigidbody> ().AddForce (direction * 2000f);
                Destroy(gameObject, 5f);
            }
            else if (phase != "Aware" && phase != "Searching")
            {
                animator.SetTrigger("IsHit");
                MakeJolted();
                MakeAngry();
            }
            else
            {
                MakeAngry();
            }
            UpdateHealth();
        }
    }

    void ShouldFire()
    {
        if (phase == "Aware")
        {
            float angryAdd = 0f;
            if (lastAngryTime != 0 && Time.time - lastAngryTime < kAngryDuration)
            {
                angryAdd = 40f;
            }
            if (playerDistance < kWillShootDistance)
            {
                animator.SetTrigger("IsAttacking");
            }
            if (playerDistance < kCloseDistance / 2)
            {
                if (Random.Range(0f, 100f) < (55f + angryAdd) * Time.deltaTime)
                {
                    animator.SetTrigger("IsAttacking");
                }
            }
            else if (playerDistance < kSeeDistance)
            {
                if (Random.Range(0f, 100f) < (30f + angryAdd) * Time.deltaTime)
                {
                    animator.SetTrigger("IsAttacking");
                }
            }
            else if (Random.Range(0f, 100f) < (15f + angryAdd) * Time.deltaTime)
            {
                animator.SetTrigger("IsAttacking");
            }
        }
    }

    bool CheckIfPlayerInSight()
    {
        Ray sight = new Ray(transform.position, playerLocalPosition);
        RaycastHit hit;
        bool didHit = Physics.Raycast(sight, out hit, 1000f);
        if (didHit && hit.transform.tag == "Player")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void UpdateHealth()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            healthBar.Die();
        }
        else
        {
            healthBar.SetValue(currentHealth * 1f / kTotalHealth);
        }
    }

    void SetHealthBar()
    {
        if (IsAngry())
        {
            healthBar.Show();
        }
        else
        {
            healthBar.Hide();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Player")
        {
            Vector3 myForward = transform.forward;
            myForward.y = 0f;
            col.transform.GetComponent<PlayerScript>().HitEnemy(Vector3.Normalize(myForward), 800f);
        }
    }

    private float RandomizeConstant(float constant){
        return Random.Range((1f - kRandomizePercent), (1f + kRandomizePercent))*constant;
    }

    private void RandomizeConstants()
    {
        kCloseDistance = RandomizeConstant(kCloseDistance);
        kSeeDistance = RandomizeConstant(kSeeDistance);
        kViewAngle = RandomizeConstant(kViewAngle);
        kMovementSpeed = RandomizeConstant(kMovementSpeed);
    }
}
