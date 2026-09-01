using UnityEngine;
using System.Collections;

public class ManholeInteract : MonoBehaviour, IInteractable
{
    [Header("Dialogue - No Crowbar")]
    public string noCrowbarLine1 = "This manhole leads to the basement, to my apartment.";
    public string noCrowbarLine2 = "It's too heavy to open bare-handed.";

    [Header("Dialogue - With Crowbar")]
    public string crowbarLine1 = "This manhole leads to the basement, to my apartment.";
    public string crowbarLine2 = "I can pry it open with this crowbar.";

    [Header("Requirement")]
    public string requiredItem = "Crowbar";

    [Header("Teleport")]
    public string nextSceneName;

    private bool hasHeardCrowbarDialogue = false;
    private bool isBusy = false;

    public void Interact()
    {
        if (isBusy) return;

        bool hasCrowbar = Inventory.Instance.HasItem(requiredItem);

        if (!hasCrowbar)
        {
            StartCoroutine(PlayDialogueSequence(noCrowbarLine1, noCrowbarLine2, null));
            return;
        }

        if (!hasHeardCrowbarDialogue)
        {
            StartCoroutine(PlayDialogueSequence(crowbarLine1, crowbarLine2, () => hasHeardCrowbarDialogue = true));
        }
        else
        {
            SceneTransition.Instance.GoToScene(nextSceneName);
        }
    }

    private IEnumerator PlayDialogueSequence(string line1, string line2, System.Action onComplete)
    {
        isBusy = true;

        yield return DialogueManager.Instance.ShowDialogueAndWait(line1);
        yield return DialogueManager.Instance.ShowDialogueAndWait(line2);

        isBusy = false;
        onComplete?.Invoke();
    }
}