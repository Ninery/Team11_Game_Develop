using UnityEngine;

public class GeneratorInteract : MonoBehaviour, IInteractable
{
    public GameObject puzzleCanvas;
    public WiringPuzzleManager puzzleManager;

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
}