using UnityEngine;

public class LightsOutDoor : MonoBehaviour, IInteractable
{
    public GameObject doorClosed;
    public GameObject doorOpen;
    public GameObject puzzleCanvas;
    public GameObject shedOverlay;

    void Start()
    {
        ApplyState(LightsOutManager.Instance.doorOpen);
        LightsOutManager.Instance.OnSolved += HandleSolved;
    }

    void OnDestroy()
    {
        if (LightsOutManager.Instance != null)
            LightsOutManager.Instance.OnSolved -= HandleSolved;
    }

    public void Interact()
    {
        if (LightsOutManager.Instance.doorOpen) return;

        puzzleCanvas.SetActive(true);
    }

    private void HandleSolved() => ApplyState(true);

    private void ApplyState(bool isOpen)
    {
        doorClosed.SetActive(!isOpen);
        doorOpen.SetActive(isOpen);

        if (shedOverlay != null)
            shedOverlay.SetActive(!isOpen);

        if (isOpen)
            foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
                col.enabled = false;
    }
}