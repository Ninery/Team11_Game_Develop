using UnityEngine;

public class MonsterPatrol : MonoBehaviour
{
    private enum MonsterState { Patrol, Alerting, Chasing }

    [Header("Patrol Bounds")]
    public Transform pointA;
    public Transform pointB;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float chaseSpeed = 4f;

    [Header("Patrol Style")]
    public bool randomPatrol = true; // true = Office style (random walk/idle), false = Park style (constant bounce)

    [Header("Walk Timing (random patrol only)")]
    public float minWalkTime = 1.5f;
    public float maxWalkTime = 4f;

    [Header("Idle Timing (random patrol only)")]
    public float minIdleTime = 1f;
    public float maxIdleTime = 5f;

    [Header("Detection")]
    public float alertDuration = 1f;
    public float loseSightDuration = 3f;
    public float searchArriveDistance = 0.3f;
    public GameObject exclamationIcon;

    private Rigidbody2D rb;
    private Animator anim;

    private MonsterState currentState = MonsterState.Patrol;
    private float direction = 1f;
    private bool isIdle = false;
    private float idleTimer = 0f;
    private float walkTimer = 0f;
    private float alertTimer = 0f;
    private float outOfSightTimer = 0f;

    private Player targetPlayer;
    private bool playerInCone = false;

    private Vector2 lastKnownPosition;
    private bool hasArrivedAtLastKnown = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (exclamationIcon != null) exclamationIcon.SetActive(false);

        if (randomPatrol)
            PickNewWalk();
        else
        {
            direction = transform.position.x <= pointA.position.x ? 1f : -1f;
            UpdateFacing();
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case MonsterState.Patrol:
                UpdatePatrol();
                break;
            case MonsterState.Alerting:
                UpdateAlert();
                break;
            case MonsterState.Chasing:
                UpdateChase();
                break;
        }
    }

    void FixedUpdate()
    {
        if (currentState == MonsterState.Alerting)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (currentState == MonsterState.Chasing)
        {
            bool visible = targetPlayer != null && playerInCone && !targetPlayer.isHidden;

            if (visible || !hasArrivedAtLastKnown)
            {
                rb.linearVelocity = new Vector2(direction * chaseSpeed, 0f);
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
            return;
        }

        if (randomPatrol && isIdle)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = new Vector2(direction * moveSpeed, 0f);
    }

    private void UpdatePatrol()
    {
        if (randomPatrol)
        {
            if (isIdle)
            {
                anim.SetBool("IsWalking", false);
                idleTimer -= Time.deltaTime;

                if (idleTimer <= 0f)
                {
                    isIdle = false;
                    PickNewWalk();
                }
                return;
            }

            anim.SetBool("IsWalking", true);

            walkTimer -= Time.deltaTime;

            bool atBoundRandom = (direction > 0f && transform.position.x >= pointB.position.x) ||
                           (direction < 0f && transform.position.x <= pointA.position.x);

            if (walkTimer <= 0f || atBoundRandom)
            {
                isIdle = true;
                idleTimer = Random.Range(minIdleTime, maxIdleTime);
            }
            return;
        }

        
        bool atBound = (direction > 0f && transform.position.x >= pointB.position.x) ||
                       (direction < 0f && transform.position.x <= pointA.position.x);

        if (atBound)
        {
            direction *= -1f;
            UpdateFacing();
        }
    }

    private void UpdateAlert()
    {
        alertTimer -= Time.deltaTime;

        if (alertTimer <= 0f)
        {
            StartChase();
        }
    }

    private void UpdateChase()
    {
        if (targetPlayer == null)
        {
            EndChase();
            return;
        }

        bool visible = playerInCone && !targetPlayer.isHidden;

        if (visible)
        {
            lastKnownPosition = targetPlayer.transform.position;
            hasArrivedAtLastKnown = false;
            outOfSightTimer = 0f;

            anim.SetBool("IsWalking", true);
            direction = targetPlayer.transform.position.x > transform.position.x ? 1f : -1f;
            UpdateFacing();
            return;
        }

        float distanceToLastKnown = Mathf.Abs(transform.position.x - lastKnownPosition.x);

        if (!hasArrivedAtLastKnown && distanceToLastKnown > searchArriveDistance)
        {
            anim.SetBool("IsWalking", true);
            direction = lastKnownPosition.x > transform.position.x ? 1f : -1f;
            UpdateFacing();
            return;
        }

        hasArrivedAtLastKnown = true;
        anim.SetBool("IsWalking", false);
        outOfSightTimer += Time.deltaTime;

        if (outOfSightTimer >= loseSightDuration)
        {
            EndChase();
        }
    }

    private void PickNewWalk()
    {
        if (transform.position.x <= pointA.position.x)
            direction = 1f;
        else if (transform.position.x >= pointB.position.x)
            direction = -1f;
        else
            direction = Random.value > 0.5f ? 1f : -1f;

        walkTimer = Random.Range(minWalkTime, maxWalkTime);
        UpdateFacing();
    }

    private void UpdateFacing()
    {
        Vector3 scale = transform.localScale;
        scale.x = -direction;
        transform.localScale = scale;
    }

    private void StartAlert()
    {
        currentState = MonsterState.Alerting;
        alertTimer = alertDuration;
        anim.SetBool("IsWalking", false);
        if (exclamationIcon != null) exclamationIcon.SetActive(true);
    }

    private void StartChase()
    {
        currentState = MonsterState.Chasing;
        outOfSightTimer = 0f;
        hasArrivedAtLastKnown = false;

        if (targetPlayer != null)
            lastKnownPosition = targetPlayer.transform.position;
    }

    private void EndChase()
    {
        currentState = MonsterState.Patrol;
        targetPlayer = null;
        isIdle = false;
        hasArrivedAtLastKnown = false;

        if (exclamationIcon != null) exclamationIcon.SetActive(false);

        if (randomPatrol)
            PickNewWalk();
        else
        {
            direction = transform.position.x <= pointA.position.x ? 1f :
                        transform.position.x >= pointB.position.x ? -1f : direction;
            UpdateFacing();
        }
    }

    public void PlayerEnteredVision(Player player)
    {
        if (player.isHidden) return;

        playerInCone = true;
        targetPlayer = player;

        if (currentState == MonsterState.Patrol)
        {
            StartAlert();
        }
    }

    public void PlayerExitedVision()
    {
        playerInCone = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && player.isHidden) return;

            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        CatManager.Instance.ResetSessionProgress();
        DeathScreen.Instance.PlayDeathSequence();
    }
}