using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CatCollectible : MonoBehaviour, IClickable, IHoverable
{
    [Header("Run Away")]
    public float jumpForce = 4f;
    public float runSpeed = 3f;
    public float gravityScale = 3f;
    public float runDuration = 3f;
    public string groundTag = "Ground";

    [Header("Scoring")]
    public bool countsTowardScore = true;

    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");

    private Animator anim;
    private Rigidbody2D rb;
    private bool collected = false;
    private bool hasLanded = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void OnHoverEnter() { }

    public void OnHoverExit() { }

    public void OnClick()
    {
        if (collected) return;
        collected = true;

        // NOT disabling the collider here anymore - it still needs to
        // physically collide with the ground while falling. The
        // "collected" check above already stops repeat clicks.

        if (countsTowardScore)
            CatManager.Instance.CollectCat();

        StartCoroutine(RunAwaySequence());
    }

    private IEnumerator RunAwaySequence()
    {
        anim.SetTrigger(JumpHash);
        hasLanded = false;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravityScale;
        rb.linearVelocity = new Vector2(0f, jumpForce);

        yield return new WaitUntil(() => hasLanded);

        rb.linearVelocity = Vector2.zero;

        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);

        anim.SetBool(IsRunningHash, true);
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(runSpeed, 0f);

        yield return new WaitForSeconds(runDuration);

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
            hasLanded = true;
    }
}