using UnityEngine;

public class StickyNote : MonoBehaviour, IInteractable
{
    [Header("References")]
    public GameObject noteDisplay;

    private bool isShown = false;

    public void Interact()
    {
        if (isShown) return;

        isShown = true;
        noteDisplay.SetActive(true);

        GetComponent<Collider2D>().enabled = false;
    }
}