using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen Instance;

    [Header("References")]
    public Image fadeImage;
    public TMP_Text deathText;

    [Header("Timing")]
    public float fadeDuration = 1f;
    public float holdDuration = 1.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Color c = fadeImage.color;
            fadeImage.color = new Color(c.r, c.g, c.b, 0f);
            deathText.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayDeathSequence()
    {
        Time.timeScale = 0f;
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        yield return StartCoroutine(FadeToBlack());

        deathText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(holdDuration);

        Time.timeScale = 1f; // restore before reload so the new scene starts unpaused

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        deathText.gameObject.SetActive(false);
        yield return StartCoroutine(FadeFromBlack());
    }

    private IEnumerator FadeToBlack()
    {
        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, 1f);
    }

    private IEnumerator FadeFromBlack()
    {
        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, 0f);
    }
}