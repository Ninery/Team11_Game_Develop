using UnityEngine;

public class MonsterOnlyTrigger : MonoBehaviour
{
    public System.Action OnMonsterEnter;
    public ChaseMonster monster;

    private bool hasFired = false;

    void Update()
    {
        if (hasFired || monster == null) return;

        if (monster.transform.position.x >= transform.position.x)
        {
            hasFired = true;
            OnMonsterEnter?.Invoke();
        }
    }
}