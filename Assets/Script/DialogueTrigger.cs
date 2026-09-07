using UnityEngine;
<<<<<<< Updated upstream
=======
using System.Collections;
>>>>>>> Stashed changes

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    public string message = "";

<<<<<<< Updated upstream
    public void Interact()
    {
        DialogueManager.Instance.ShowDialogue(message);
=======
    [Header("Optional second line (leave blank to skip)")]
    public string secondMessage = "";

    public void Interact()
    {
        if (string.IsNullOrEmpty(secondMessage))
        {
            DialogueManager.Instance.ShowDialogue(message);
        }
        else
        {
            StartCoroutine(PlayBothLines());
        }
    }

    private IEnumerator PlayBothLines()
    {
        yield return DialogueManager.Instance.ShowDialogueAndWait(message);
        yield return DialogueManager.Instance.ShowDialogueAndWait(secondMessage);
>>>>>>> Stashed changes
    }
}