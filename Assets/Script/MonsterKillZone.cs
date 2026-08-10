using UnityEngine;

public class MonsterKillZone : MonoBehaviour
{
    public MonsterPatrol monster;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && player.isHidden) return;

            monster.KillPlayer();
        }
    }
}