using UnityEngine;

public class ItemCollectible : MonoBehaviour, IInteractable
{
<<<<<<< Updated upstream
    [Header("Item Granted")]
    public string itemName; 
=======
    public string itemName;

    [Header("Optional: trigger a checkpoint on pickup")]
    public bool triggersCheckpoint = false;
    public Transform checkpointRespawnPoint; // if empty, uses this item's own position
>>>>>>> Stashed changes

    private bool collected = false;

    public void Interact()
    {
        if (collected) return;

        Inventory.Instance.AddItem(itemName);
        collected = true;

<<<<<<< Updated upstream
        gameObject.SetActive(false); 
=======
        if (triggersCheckpoint)
        {
            Vector3 spawnPos = checkpointRespawnPoint != null ? checkpointRespawnPoint.position : transform.position;
            CheckpointManager.Instance.SetCheckpoint(spawnPos, Inventory.Instance.GetHeldItems());
        }

        gameObject.SetActive(false);
>>>>>>> Stashed changes
    }
}