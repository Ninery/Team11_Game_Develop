using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    public string message = "";

    public void Interact()
    {
        DialogueManager.Instance.ShowDialogue(message);
    }
}