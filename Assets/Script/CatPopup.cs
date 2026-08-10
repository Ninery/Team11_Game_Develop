using UnityEngine;
using TMPro;
using System.Collections;

public class CatPopup : MonoBehaviour
{
    public static CatPopup Instance;

    [Header("References")]
    public CanvasGroup canvasGroup;
    public TMP_Text countText;

    [Header("Timing")]
    public float displayDuration = 3f;
    public float fadeDuration = 1f;

    private Coroutine activeCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            canvasGroup.alpha = 0f;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowPopup(int collected, int total)
    {
        countText.text = collected + " / " + total;

        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = StartCoroutine(ShowAndFade());
    }

    private IEnumerator ShowAndFade()
    {
        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(displayDuration);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}