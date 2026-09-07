using UnityEngine;
using System.Collections;

public class LiftEscapeSequence : MonoBehaviour
{
    [Header("References")]
    public LiftInteract liftButton;
    public Player player;
    public ChaseMonster chaseMonster;

    [Header("Lift Positions")]
    public Transform liftCenterPoint;
    public MonsterOnlyTrigger doorCloseTrigger;
    public MonsterOnlyTrigger monsterStandTrigger;

    [Header("Door Sorting (covers player once closing starts)")]
    public SpriteRenderer doorSpriteRenderer;
    public int doorClosingSortingOrder = 10;

    [Header("Floor Lights")]
    public string floorLightStateName = "LiftLights";

    [Header("Chase Speed Up")]
    public float speedUpMultiplier = 2f;

    [Header("Ending")]
    public string endingSceneName;
    public float delayBeforeFade = 1f;

    private bool sequenceStarted = false;

    void OnEnable()
    {
        if (doorCloseTrigger != null)
            doorCloseTrigger.OnMonsterEnter += StartDoorClosing;

        if (monsterStandTrigger != null)
            monsterStandTrigger.OnMonsterEnter += StopMonsterAtDoor;
    }

    void OnDisable()
    {
        if (doorCloseTrigger != null)
            doorCloseTrigger.OnMonsterEnter -= StartDoorClosing;

        if (monsterStandTrigger != null)
            monsterStandTrigger.OnMonsterEnter -= StopMonsterAtDoor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (sequenceStarted) return;
        if (!other.CompareTag("Player")) return;
        if (liftButton == null || !liftButton.DoorOpened) return;

        StartSequence();
    }

    private void StartSequence()
    {
        sequenceStarted = true;

        player.transform.position = liftCenterPoint.position;
        player.canMove = false;
        player.isInvulnerable = true;

        if (chaseMonster != null)
            chaseMonster.chaseSpeed *= speedUpMultiplier;
    }

    private void StartDoorClosing()
    {
        if (!sequenceStarted) return;

        if (doorSpriteRenderer != null)
            doorSpriteRenderer.sortingOrder = doorClosingSortingOrder;

        if (liftButton != null)
            liftButton.CloseDoor();

        if (chaseMonster != null)
            chaseMonster.DisableKillTrigger();
    }

    private void StopMonsterAtDoor()
    {
        if (!sequenceStarted) return;

        if (chaseMonster != null)
            chaseMonster.StopChase();

        if (liftButton != null)
            liftButton.PlayFloorLights(floorLightStateName);

        StartCoroutine(TriggerEnding());
    }

    private IEnumerator TriggerEnding()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        if (!string.IsNullOrEmpty(endingSceneName) && SceneTransition.Instance != null)
        {
            SceneTransition.Instance.GoToScene(endingSceneName);
        }
    }
}