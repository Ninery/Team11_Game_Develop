using UnityEngine;
using UnityEngine.InputSystem;

public class CabinetHide : MonoBehaviour, IInteractable
{
    [Header("References")]
    public Transform player;
    public SpriteRenderer playerSprite;
    public Transform hideSpot;
    public Transform frontSpot;

    public bool isHiding = false;

    private Player playerScript;
    private Rigidbody2D playerRb;
    private bool skipNextEInput = false;

    void Awake()
    {
        // auto-find the player by tag, so each cabinet prefab instance
        // doesn't need the Player field manually dragged in every time
        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null)
            {
                player = found.transform;
                playerSprite = found.GetComponent<SpriteRenderer>();
            }
        }
    }

    public void Interact()
    {
        if (!isHiding)
        {
            StartHiding();
        }
    }

    void Update()
    {
        if (isHiding)
        {
            if (skipNextEInput)
            {
                skipNextEInput = false;
                return;
            }

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                StopHiding();
            }
        }
    }

    private void StartHiding()
    {
        isHiding = true;
        skipNextEInput = true;
        playerScript = player.GetComponent<Player>();
        playerRb = player.GetComponent<Rigidbody2D>();

        player.position = hideSpot.position;
        playerSprite.enabled = false;
        playerScript.canMove = false;
        playerScript.isHidden = true; // future monster scripts check this

        playerRb.simulated = false;

        playerScript.suppressBubbleControl = true;
        playerScript.bubbleE.SetActive(true);
    }

    private void StopHiding()
    {
        isHiding = false;

        player.position = frontSpot.position;
        playerRb.simulated = true;
        playerSprite.enabled = true;
        playerScript.canMove = true;
        playerScript.isHidden = false;

        playerScript.suppressBubbleControl = false;
    }
}