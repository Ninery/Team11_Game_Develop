using UnityEngine;
using System.Collections;
using TMPro;

public class BasementMonsterPatrol : MonoBehaviour
{
    [Header("Follow")]
    public Transform player;
    public float moveSpeed = 2f;

    [Header("Boundary (monster cannot cross past this X position)")]
    public Transform boundaryPoint;

    [Header("Stun")]
    public float stunDuration = 5f;
    public TMP_Text stunCountdownText;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isStunned = false;
    private float blockedDirection = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (stunCountdownText != null)
            stunCountdownText.gameObject.SetActive(false);

        if (boundaryPoint != null)
        {
            // whichever side the monster starts on, moving TOWARD the boundary
            // and past it is the forbidden direction - retreating is always fine
            blockedDirection = transform.position.x <= boundaryPoint.position.x ? 1f : -1f;
        }
    }

    void Update()
    {
        if (isStunned || !FlashlightPickup.HasFlashlight || player == null)
        {
            anim.SetBool("IsWalking", false);
            return;
        }

        float direction = player.position.x > transform.position.x ? 1f : -1f;

        if (WouldCrossBoundary(direction))
        {
            anim.SetBool("IsWalking", false);
            return;
        }

        anim.SetBool("IsWalking", true);
        UpdateFacing(direction);
    }

    void FixedUpdate()
    {
        if (isStunned || !FlashlightPickup.HasFlashlight || player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float direction = player.position.x > transform.position.x ? 1f : -1f;

        if (WouldCrossBoundary(direction))
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = new Vector2(direction * moveSpeed, 0f);
    }

    private bool WouldCrossBoundary(float direction)
    {
        if (boundaryPoint == null) return false;
        if (direction != blockedDirection) return false; // retreating is always allowed

        return (blockedDirection > 0f && transform.position.x >= boundaryPoint.position.x) ||
               (blockedDirection < 0f && transform.position.x <= boundaryPoint.position.x);
    }

    public bool IsStunned()
    {
        return isStunned;
    }

    public void TriggerStun()
    {
        if (isStunned) return;

        StartCoroutine(StunSequence());
    }

    private IEnumerator StunSequence()
    {
        isStunned = true;
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("IsWalking", false);
        anim.Play("BasementMonsterStun", 0, 0f);

        float remaining = stunDuration;

        if (stunCountdownText != null)
            stunCountdownText.gameObject.SetActive(true);

        while (remaining > 0f)
        {
            if (stunCountdownText != null)
                stunCountdownText.text = Mathf.CeilToInt(remaining).ToString();

            yield return null;
            remaining -= Time.deltaTime;
        }

        if (stunCountdownText != null)
            stunCountdownText.gameObject.SetActive(false);

        isStunned = false;
    }

    private void UpdateFacing(float direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = -direction;
        transform.localScale = scale;

        if (stunCountdownText != null)
        {
            Vector3 textScale = stunCountdownText.transform.localScale;
            textScale.x = Mathf.Abs(textScale.x) * Mathf.Sign(scale.x);
            stunCountdownText.transform.localScale = textScale;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isStunned) return;

        if (other.CompareTag("Player"))
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        CatManager.Instance.ResetSessionProgress();
        DeathScreen.Instance.PlayDeathSequence();
    }
}