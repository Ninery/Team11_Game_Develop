using UnityEngine;
using System.Collections;

public class LiftInteract : MonoBehaviour, IInteractable
{
    public static bool DoorHasOpened = false;

    [Header("Power Check")]
    public string noPowerMessage = "There's no power.";

    [Header("Lever Check")]
    public WallNote wallNote;

    [Header("Animator (holds both LiftDoors and LiftLights clips)")]
    public Animator liftAnim;
    public string doorStateName = "LiftDoors";

    [Header("Interior")]
    public GameObject liftInterior;

    [Header("Sound")]
    public AudioSource liftRunAudio;

    public bool DoorOpened => DoorHasOpened;
    private bool isAnimating = false;

    void Start()
    {
        if (DoorHasOpened)
        {
            if (liftInterior != null) liftInterior.SetActive(true);
            liftAnim.speed = 1f;
            liftAnim.Play(doorStateName, 0, 1f);
        }
    }

    public void Interact()
    {
        if (DoorOpened || isAnimating) return;

        if (!WiringPuzzleManager.IsSolved)
        {
            DialogueManager.Instance.ShowDialogue(noPowerMessage);
            return;
        }

        if (wallNote != null && !wallNote.AreLeversCorrect())
        {
            DialogueManager.Instance.ShowDialogue(noPowerMessage);
            return;
        }

        OpenDoor();
    }

    private void OpenDoor()
    {
        DoorHasOpened = true;
        isAnimating = true;

        if (liftInterior != null) liftInterior.SetActive(true);

        if (liftRunAudio != null && liftRunAudio.clip != null)
            liftRunAudio.PlayOneShot(liftRunAudio.clip);

        liftAnim.speed = 1f;
        liftAnim.Play(doorStateName, 0, 0f);

        StartCoroutine(WaitForOpenFinish());
    }

    private IEnumerator WaitForOpenFinish()
    {
        yield return null;
        yield return new WaitForSeconds(liftAnim.GetCurrentAnimatorStateInfo(0).length);
        isAnimating = false;
    }

    public void CloseDoor()
    {
        StartCoroutine(CloseDoorReverse());
    }

    private IEnumerator CloseDoorReverse()
    {
        liftAnim.Play(doorStateName, 0, 1f);
        liftAnim.speed = 0f;

        float clipLength = liftAnim.GetCurrentAnimatorStateInfo(0).length;
        float t = 1f;

        while (t > 0f)
        {
            liftAnim.Play(doorStateName, 0, t);
            yield return null;
            t -= Time.deltaTime / clipLength;
        }

        
        liftAnim.Play(doorStateName, 0, 0f);
        liftAnim.speed = 0f;
    }

    public void PlayFloorLights(string floorLightStateName)
    {
        if (liftAnim != null)
        {
            liftAnim.speed = 1f; 
            liftAnim.Play(floorLightStateName, 0, 0f);
        }
    }
}