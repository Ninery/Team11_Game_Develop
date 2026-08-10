using UnityEngine;

public class CoatHanger : MonoBehaviour, IInteractable
{
    private bool collected = false;

    public void Interact()
    {
        if (collected) return;

        Inventory.Instance.AddItem("CoatNote");
        collected = true;

        GetComponent<Collider2D>().enabled = false;
    }
}