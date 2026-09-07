using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    [Header("Checkpoint Text Popup")]
    public CanvasGroup checkpointTextGroup;
    public TMP_Text checkpointText;
    public string checkpointMessage = "Checkpoint Saved";
    public float displayDuration = 2f;
    public float fadeDuration = 1f;

    [Header("Checkpoint Icon (animated)")]
    public Animator checkpointIconAnim;
    public string checkpointAnimStateName = "Checkpoint";

    private string checkpointScene = "";
    private Vector3 checkpointPosition;
    private List<string> checkpointItems = new List<string>();
    private bool hasCheckpoint = false;

    private Coroutine popupCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (checkpointTextGroup != null) checkpointTextGroup.alpha = 0f;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(Vector3 position, IEnumerable<string> heldItems)
    {
        checkpointScene = SceneManager.GetActiveScene().name;
        checkpointPosition = position;
        checkpointItems = new List<string>(heldItems);
        hasCheckpoint = true;

        if (CatManager.Instance != null)
            CatManager.Instance.CommitSessionProgress();

        ShowPopup();
    }

    public void ShowSceneEntryPopup()
    {
        ShowPopup();
    }

    public bool TryGetCheckpoint(out Vector3 position, out List<string> items)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (hasCheckpoint && checkpointScene == currentScene)
        {
            position = checkpointPosition;
            items = checkpointItems;
            return true;
        }

        position = Vector3.zero;
        items = null;
        return false;
    }

    private void ShowPopup()
    {
        if (checkpointTextGroup == null) return;

        if (checkpointText != null)
            checkpointText.text = checkpointMessage;

        if (checkpointIconAnim != null)
            checkpointIconAnim.Play(checkpointAnimStateName, 0, 0f);

        if (popupCoroutine != null)
            StopCoroutine(popupCoroutine);

        popupCoroutine = StartCoroutine(ShowAndFade());
    }

    private IEnumerator ShowAndFade()
    {
        checkpointTextGroup.alpha = 1f;
        yield return new WaitForSeconds(displayDuration);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            checkpointTextGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        checkpointTextGroup.alpha = 0f;
    }
}