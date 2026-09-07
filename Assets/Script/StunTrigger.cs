using UnityEngine;
using UnityEngine.InputSystem;

public class StunTrigger : MonoBehaviour
{
    public StunDetector stunDetector;
    public BasementMonsterPatrol monster;
    public FlashlightStunEffect flashlightEffect;
    public GameObject stunPromptText;

    void Update()
    {
        if (stunDetector == null || monster == null) return;

        bool canStun = stunDetector.IsMonsterInRange()
                       && !monster.IsStunned()
                       && (flashlightEffect == null || !flashlightEffect.IsOnCooldown());

        if (stunPromptText != null)
            stunPromptText.SetActive(canStun);

        if (canStun && Mouse.current.leftButton.wasPressedThisFrame)
        {
            monster.TriggerStun();

            if (flashlightEffect != null)
                flashlightEffect.PlayStunFlashEffect();
        }
    }
}