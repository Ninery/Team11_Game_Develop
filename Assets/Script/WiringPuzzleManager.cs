using UnityEngine;
using System.Collections;

public class WiringPuzzleManager : MonoBehaviour
{
    public static bool IsSolved = false;

    public WireCog[] cogs;
    public GameObject puzzleCanvas;
    public GeneratorInteract generatorInteract;

    [Header("On Solve")]
    public GameObject globalLight;
    public GameObject[] playerLights;
    public GameObject wallNote;
    public float autoCloseDelay = 2f;

    [Header("Sound")]
    public AudioSource wireConnectAudio;

    [Header("Key/Door (should already be past this point by checkpoint)")]
    public DoorInteract keyDoor;
    public GameObject keyCollectible;

    [Header("Checkpoint")]
    public Transform checkpointRespawnPoint;

    private int solvedCount = 0;
    private Player currentPlayer;

    void Start()
    {
        foreach (var cog in cogs)
        {
            cog.OnSolved += HandleCogSolved;
        }

        if (IsSolved)
        {
            ApplySolvedStateInstantly();
        }
    }

    void OnEnable()
    {
        currentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        currentPlayer.canMove = false;
    }

    public void ClosePuzzle()
    {
        if (currentPlayer != null)
            currentPlayer.canMove = true;

        if (puzzleCanvas != null)
            puzzleCanvas.SetActive(false);
        else
            gameObject.SetActive(false);
    }

    private void HandleCogSolved()
    {
        solvedCount++;

        if (wireConnectAudio != null && wireConnectAudio.clip != null)
            wireConnectAudio.PlayOneShot(wireConnectAudio.clip);

        if (solvedCount >= cogs.Length)
        {
            OnPuzzleComplete();
        }
    }

    private void OnPuzzleComplete()
    {
        IsSolved = true;
        ApplySolvedStateInstantly();

        if (generatorInteract != null)
            generatorInteract.DisableCollider();
            
        if (generatorInteract != null)
            generatorInteract.PlayLightOnSound();

        if (checkpointRespawnPoint != null && CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.SetCheckpoint(
                checkpointRespawnPoint.position,
                Inventory.Instance.GetHeldItems());
        }

        StartCoroutine(AutoCloseAfterDelay());
    }

    public void ApplySolvedStateInstantly()
    {
        if (globalLight != null)
            globalLight.SetActive(true);

        foreach (var light in playerLights)
        {
            if (light != null)
                light.SetActive(false);
        }

        if (wallNote != null)
            wallNote.SetActive(true);

        if (keyDoor != null)
            keyDoor.ForceOpen();

        if (keyCollectible != null)
            keyCollectible.SetActive(false);
    }

    private IEnumerator AutoCloseAfterDelay()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        ClosePuzzle();
    }
}