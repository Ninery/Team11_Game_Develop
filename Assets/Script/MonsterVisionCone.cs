using UnityEngine;

public class MonsterVisionCone : MonoBehaviour
{
    public MonsterPatrol monster;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
                monster.PlayerEnteredVision(player);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            monster.PlayerExitedVision();
        }
    }
}