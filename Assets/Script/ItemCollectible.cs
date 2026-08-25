using UnityEngine;

public class ItemCollectible : MonoBehaviour, IInteractable
{
    [Header("Item Granted")]
    public string itemName; 

    private bool collected = false;

    public void Interact()
    {
        if (collected) return;

        Inventory.Instance.AddItem(itemName);
        collected = true;

        gameObject.SetActive(false); 
    }
}