using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    protected static Transform player;
    protected static UI ui;

    protected Animator animator;
    protected Bar healthBar;
    protected EnemyEmoteCanvasScript enemyEmoteCanvas;

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

    protected void Start()
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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
