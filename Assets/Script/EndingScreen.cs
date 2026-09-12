using UnityEngine;

public class EndingScreen : MonoBehaviour
{
    [Header("Ending 1 - Not All Cats Collected")]
    public GameObject ending1Canvas;
    public AudioSource ending1AudioSource;

    [Header("Ending 2 - All Cats Collected")]
    public GameObject ending2Canvas;
    public AudioSource ending2AudioSource;

    void Start()
    {
        bool gotAllCats = CatManager.Instance != null &&
                           CatManager.Instance.GetTotalCollected() >= CatManager.Instance.totalCats;

        if (ending1Canvas != null)
            ending1Canvas.SetActive(!gotAllCats);

        if (ending2Canvas != null)
            ending2Canvas.SetActive(gotAllCats);

        if (ending1AudioSource != null)
            ending1AudioSource.Stop();

        if (ending2AudioSource != null)
            ending2AudioSource.Stop();

        if (gotAllCats)
        {
            if (ending2AudioSource != null &&
                ending2AudioSource.clip != null)
            {
                ending2AudioSource.Play();
            }
        }
        else
        {
            if (ending1AudioSource != null &&
                ending1AudioSource.clip != null)
            {
                ending1AudioSource.Play();
            }
        }
    }

    public void RestartToMenu()
    {
        WiringPuzzleManager.IsSolved = false;
        LiftInteract.DoorHasOpened = false;
        FlashlightPickup.HasFlashlight = false;
        DoorInteract.ResetOpenedDoors();
        PigeonNest.ResetTriggeredNests();

        if (LightsOutManager.Instance != null)
            LightsOutManager.Instance.ResetPuzzle();

        if (ending1AudioSource != null)
            ending1AudioSource.Stop();

        if (ending2AudioSource != null)
            ending2AudioSource.Stop();

        SceneTransition.Instance.GoToScene("StartMenu");
    }
}