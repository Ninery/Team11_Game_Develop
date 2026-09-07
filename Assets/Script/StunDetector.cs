using UnityEngine;

public class StunDetector : MonoBehaviour
{
    private bool monsterInRange = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
            monsterInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
            monsterInRange = false;
    }

    public bool IsMonsterInRange()
    {
        return monsterInRange;
    }
}