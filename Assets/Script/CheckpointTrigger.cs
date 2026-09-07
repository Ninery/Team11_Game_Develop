using UnityEngine;
using System.Collections.Generic;

public class CheckpointTrigger : MonoBehaviour
{
    [Header("Drag this checkpoint's own GameObject here")]
    public GameObject checkpointIdentity;

    private static HashSet<int> triggeredCheckpoints = new HashSet<int>();

    void Start()
    {
        if (checkpointIdentity != null && triggeredCheckpoints.Contains(checkpointIdentity.GetInstanceID()))
            gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (checkpointIdentity == null) return;
        if (triggeredCheckpoints.Contains(checkpointIdentity.GetInstanceID())) return;

        triggeredCheckpoints.Add(checkpointIdentity.GetInstanceID());

        CheckpointManager.Instance.SetCheckpoint(transform.position, Inventory.Instance.GetHeldItems());

        gameObject.SetActive(false);
    }
}