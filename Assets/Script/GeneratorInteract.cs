using UnityEngine;
using System.Collections;

public class GeneratorInteract : MonoBehaviour, IInteractable
{
    public GameObject puzzleCanvas;
    public WiringPuzzleManager puzzleManager;

    [Header("Sound")]
    public AudioSource lightOnAudio1;
    public AudioSource lightOnAudio2;

    void Start()
    {
        if (WiringPuzzleManager.IsSolved)
        {
            if (puzzleManager != null)
                puzzleManager.ApplySolvedStateInstantly();

            DisableCollider();
        }
    }

    public void Interact()
    {
        if (WiringPuzzleManager.IsSolved) return;

        if (puzzleCanvas != null)
            puzzleCanvas.SetActive(true);
    }

    public void DisableCollider()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
    }

    public void PlayLightOnSound()
    {
        StartCoroutine(PlayLightOnSounds());
    }

    private IEnumerator PlayLightOnSounds()
    {
        if (lightOnAudio1 != null && lightOnAudio1.clip != null)
        {
            lightOnAudio1.Play();
            yield return new WaitForSeconds(lightOnAudio1.clip.length);
        }

        if (lightOnAudio2 != null && lightOnAudio2.clip != null)
        {
            lightOnAudio2.Play();
        }
    }
}