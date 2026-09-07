using UnityEngine;

public class DarknessBlocker : MonoBehaviour
{
    public string message = "It's too dark to go any further.";
    private bool hasShownThisApproach = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (hasShownThisApproach) return;

        hasShownThisApproach = true;
        DialogueManager.Instance.ShowDialogue(message);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        hasShownThisApproach = false;
    }
}