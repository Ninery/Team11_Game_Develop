using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Interaction")]
    public GameObject bubbleE;

    [Header("Flashlight")]
    public Transform flashlightTransform;

    [Header("State")]
    public bool canMove = true;
    public bool suppressBubbleControl = false;
    public bool isHidden = false;
    public bool isInvulnerable = false;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private float moveInput;

    private List<IInteractable> nearby = new List<IInteractable>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        bubbleE.SetActive(false);

        if (CheckpointManager.Instance != null &&
            CheckpointManager.Instance.TryGetCheckpoint(out Vector3 savedPosition, out _))
        {
            transform.position = savedPosition;
        }
    }

    void Update()
    {
        moveInput = 0f;

        if (canMove)
        {
            if (Keyboard.current.aKey.isPressed)
                moveInput = -1f;
            else if (Keyboard.current.dKey.isPressed)
                moveInput = 1f;

            if (moveInput != 0)
            {
                sr.flipX = moveInput < 0;

                if (flashlightTransform != null)
                {
                    Vector3 scale = flashlightTransform.localScale;
                    scale.x = moveInput < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
                    flashlightTransform.localScale = scale;
                }
            }
        }

        anim.SetBool("IsWalking", moveInput != 0);

        if (!suppressBubbleControl)
            bubbleE.SetActive(nearby.Count > 0 && canMove);

        if (Keyboard.current.eKey.wasPressedThisFrame && nearby.Count > 0 && canMove)
        {
            nearby[0].Interact();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
            nearby.Add(interactable);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
            nearby.Remove(interactable);
    }
}