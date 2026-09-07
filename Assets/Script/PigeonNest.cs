using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PigeonNest : MonoBehaviour, IInteractable
{
    [Header("Unique ID for this nest")]
    public string nestId = "PigeonNest1";

    [Header("Dialogue (before seed bag placed)")]
    public string firstLine = "hm a crowbar, i wonder if it is useful";
    public string secondLine = "i cant reach it";

    [Header("Trigger Requirement")]
    public string requiredItem = "SeedBag";

    [Header("Pigeon")]
    public Animator pigeonAnim;
    public Transform pigeonTransform;
    public Transform pigeonLandSpot;
    public float pigeonFlySpeed = 2f;

    [Header("Placed Seed Bag Visual")]
    public GameObject placedSeedBag;

    [Header("Crowbar")]
    public Rigidbody2D crowbarRb;
    public Collider2D crowbarPickupCollider;
    public Transform crowbarLandSpot;
    public float crowbarGravityScale = 3f;

    private static readonly int FlyHash = Animator.StringToHash("Fly");
    private static readonly int IsEatingHash = Animator.StringToHash("IsEating");

    private static HashSet<string> triggeredNests = new HashSet<string>();

    private bool triggered = false;

    void Start()
    {
        if (triggeredNests.Contains(nestId))
        {
            triggered = true;

            if (placedSeedBag != null)
                placedSeedBag.SetActive(true);

            pigeonTransform.position = pigeonLandSpot.position;
            pigeonAnim.SetBool(IsEatingHash, true);

            crowbarRb.position = crowbarLandSpot.position;
            crowbarRb.bodyType = RigidbodyType2D.Kinematic;
            crowbarRb.gravityScale = 0f;

            GetComponent<Collider2D>().enabled = false;
        }
    }

    public void Interact()
    {
        if (triggered) return;

        if (Inventory.Instance.HasItem(requiredItem))
        {
            triggered = true;
            triggeredNests.Add(nestId);

            Inventory.Instance.RemoveItem(requiredItem);
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(TriggerSequence());
        }
        else
        {
            StartCoroutine(NoItemDialogueSequence());
        }
    }

    private IEnumerator NoItemDialogueSequence()
    {
        yield return DialogueManager.Instance.ShowDialogueAndWait(firstLine);
        yield return DialogueManager.Instance.ShowDialogueAndWait(secondLine);
    }

    private IEnumerator TriggerSequence()
    {
        if (placedSeedBag != null)
            placedSeedBag.SetActive(true);

        pigeonAnim.SetTrigger(FlyHash);
        StartCoroutine(FlyPigeonDown());

        crowbarRb.bodyType = RigidbodyType2D.Dynamic;
        crowbarRb.gravityScale = crowbarGravityScale;

        while (crowbarRb.position.y > crowbarLandSpot.position.y)
        {
            yield return null;
        }

        crowbarRb.position = new Vector2(crowbarRb.position.x, crowbarLandSpot.position.y);
        crowbarRb.linearVelocity = Vector2.zero;
        crowbarRb.gravityScale = 0f;
        crowbarRb.bodyType = RigidbodyType2D.Kinematic;

        crowbarPickupCollider.enabled = true;
    }

    private IEnumerator FlyPigeonDown()
    {
        while (Vector3.Distance(pigeonTransform.position, pigeonLandSpot.position) > 0.02f)
        {
            pigeonTransform.position = Vector3.MoveTowards(
                pigeonTransform.position,
                pigeonLandSpot.position,
                pigeonFlySpeed * Time.deltaTime
            );
            yield return null;
        }

        pigeonTransform.position = pigeonLandSpot.position;
        pigeonAnim.SetBool(IsEatingHash, true);
    }
}