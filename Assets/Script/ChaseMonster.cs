using UnityEngine;
using System.Collections;
using TMPro;

public class ChaseMonster : MonoBehaviour
{
    [Header("Movement")]
    public Transform player;
    public float chaseSpeed = 3f;
    public float delayBeforeChase = 1.5f;

    [Header("UI")]
    public GameObject runText;

    [Header("Kill Detection (the box trigger collider, not the small ground one)")]
    public Collider2D killTrigger;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isChasing = false;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isChasing = false;

        UpdateFacing(1f);

        if (runText != null)
            runText.SetActive(true);

        if (killTrigger != null)
            killTrigger.enabled = true;

        StartCoroutine(StartChaseAfterDelay());
    }

    private IEnumerator StartChaseAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeChase);
        isChasing = true;
    }

    void Update()
    {
        if (!isChasing || player == null) return;

        float direction = player.position.x > transform.position.x ? 1f : -1f;
        UpdateFacing(direction);
    }

    void FixedUpdate()
    {
        if (!isChasing || player == null)
        {
            if (rb != null) rb.linearVelocity = Vector2.zero;
            if (anim != null) anim.SetBool("IsWalking", false);
            return;
        }

        if (anim != null) anim.SetBool("IsWalking", true);

        float direction = player.position.x > transform.position.x ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * chaseSpeed, 0f);
    }

    private void UpdateFacing(float direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = -direction;
        transform.localScale = scale;
    }

    public void StopChase()
    {
        isChasing = false;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (anim != null)
            anim.SetBool("IsWalking", false);

        if (runText != null)
            runText.SetActive(false);
    }

    public void DisableKillTrigger()
    {
        if (killTrigger != null)
            killTrigger.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Player p = other.GetComponent<Player>();
        if (p != null && p.isInvulnerable) return;

        KillPlayer();
    }

    public void KillPlayer()
    {
        CatManager.Instance.ResetSessionProgress();
        DeathScreen.Instance.PlayDeathSequence();
    }
}