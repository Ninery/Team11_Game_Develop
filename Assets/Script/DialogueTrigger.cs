using UnityEngine;
using System.Collections;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    public string message = "";

    [Header("Optional second line (leave blank to skip)")]
    public string secondMessage = "";

    [Header("Sound")]
    public AudioSource audioSource;

    public void Interact()
    {
        if (audioSource != null && audioSource.clip != null)
            audioSource.PlayOneShot(audioSource.clip);

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
    }
}