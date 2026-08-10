using System.Collections;
using UnityEngine;

public class CatCollectible : MonoBehaviour, IClickable, IHoverable
{
    [Header("Run Away")]
    public float jumpForce = 4f;
    public float runSpeed = 3f;
    public float gravityScale = 3f;
    public float jumpAirTime = 0.4f;
    public float runDuration = 3f;

    private Animator anim;
    private Rigidbody2D rb;
    private bool collected = false;

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

        GetComponent<Collider2D>().enabled = false;

        CatManager.Instance.CollectCat();
        StartCoroutine(RunAwaySequence());
    }

    private IEnumerator RunAwaySequence()
    {
        anim.SetTrigger("Jump");
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravityScale;
        rb.linearVelocity = new Vector2(0f, jumpForce);

        yield return new WaitForSeconds(jumpAirTime);

        // face right before running off, regardless of idle pose
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);

        anim.SetBool("IsRunning", true);
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(runSpeed, 0f);

        yield return new WaitForSeconds(runDuration);

        Destroy(gameObject);
    }
}