using UnityEngine;

public class FlashlightPickup : MonoBehaviour, IInteractable
{
    public static bool HasFlashlight = false;

    [Header("Flashlight Light (child of Player, starts disabled)")]
    public GameObject flashlightLight;

    [Header("Darkness Blockers to remove on pickup")]
    public GameObject blockerLeft;
    public GameObject blockerRight;

    void Start()
    {
        if (HasFlashlight)
            ApplyCollectedState();
    }

    public void Interact()
    {
        if (HasFlashlight) return;

        HasFlashlight = true;
        ApplyCollectedState();
    }

    private void ApplyCollectedState()
    {
        if (flashlightLight != null)
            flashlightLight.SetActive(true);

        if (blockerLeft != null)
            blockerLeft.SetActive(false);

        if (blockerRight != null)
            blockerRight.SetActive(false);

        gameObject.SetActive(false);
    }
}