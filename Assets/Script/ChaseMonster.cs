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

    [Header("Kill Detection (the box trigger, not the small ground one)")]
    public Collider2D killTrigger;

    [Header("Footstep Sound")]
    public AudioSource footstepAudioSource;
    public AudioClip runClip;

    [Header("Chase BGM")]
    public AudioClip chaseBGM;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isChasing = false;

    private AudioClip normalBGM;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isChasing = false;

        if (GlobalBGM.Instance != null &&
            GlobalBGM.Instance.audioSource != null)
        {
            normalBGM = GlobalBGM.Instance.audioSource.clip;
        }

        UpdateFacing(1f);

        if (runText != null)
            runText.SetActive(true);

        if (killTrigger != null)
            killTrigger.enabled = true;

        if (footstepAudioSource != null)
        {
            footstepAudioSource.Stop();
            footstepAudioSource.clip = runClip;
        }

        StartCoroutine(StartChaseAfterDelay());
    }

    private IEnumerator StartChaseAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeChase);

        isChasing = true;

        if (player != null)
        {
            Player playerScript = player.GetComponent<Player>();

            if (playerScript != null)
                playerScript.SetChaseAudio(true);
        }

        if (footstepAudioSource != null &&
            footstepAudioSource.enabled &&
            footstepAudioSource.gameObject.activeInHierarchy &&
            footstepAudioSource.clip != null)
        {
            footstepAudioSource.Play();
        }

        if (GlobalBGM.Instance != null &&
            GlobalBGM.Instance.audioSource != null &&
            chaseBGM != null)
        {
            GlobalBGM.Instance.audioSource.Stop();
            GlobalBGM.Instance.audioSource.clip = chaseBGM;
            GlobalBGM.Instance.audioSource.Play();
        }
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
            if (rb != null)
                rb.linearVelocity = Vector2.zero;

            if (anim != null)
                anim.SetBool("IsWalking", false);

            return;
        }

        if (anim != null)
            anim.SetBool("IsWalking", true);

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

        if (footstepAudioSource != null)
            footstepAudioSource.Stop();

        if (player != null)
        {
            Player playerScript = player.GetComponent<Player>();

            if (playerScript != null)
                playerScript.SetChaseAudio(false);
        }

        RestoreNormalBGM();

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
        if (footstepAudioSource != null)
            footstepAudioSource.Stop();

        RestoreNormalBGM();

        CatManager.Instance.ResetSessionProgress();
        DeathScreen.Instance.PlayDeathSequence();
    }

    private void RestoreNormalBGM()
    {
        if (GlobalBGM.Instance != null &&
            GlobalBGM.Instance.audioSource != null &&
            normalBGM != null)
        {
            GlobalBGM.Instance.audioSource.Stop();
            GlobalBGM.Instance.audioSource.clip = normalBGM;
            GlobalBGM.Instance.audioSource.Play();
        }
    }
}