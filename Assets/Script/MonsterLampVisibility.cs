using UnityEngine;
using System.Collections;

public class MonsterLampVisibility : MonoBehaviour
{
    public string lampTag = "LampLight";
    public float fadeSpeed = 3f;
<<<<<<< Updated upstream
    public float startingAlpha = 0f; // alpha while NOT in a lamp's light - used at boot AND every fade-out
=======
    public float startingAlpha = 0f;
>>>>>>> Stashed changes

    private SpriteRenderer sr;
    private Coroutine fadeCoroutine;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            Color c = sr.color;
            c.a = startingAlpha;
            sr.color = c;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
<<<<<<< Updated upstream
=======
        if (!gameObject.activeInHierarchy) return;

>>>>>>> Stashed changes
        if (other.CompareTag(lampTag))
            StartFade(1f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
<<<<<<< Updated upstream
        if (other.CompareTag(lampTag))
            StartFade(startingAlpha);
=======
        if (!gameObject.activeInHierarchy) return;

        if (other.CompareTag(lampTag))
            StartFade(0f);
>>>>>>> Stashed changes
    }

    private void StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeTo(targetAlpha));
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        if (sr == null) yield break;

        while (Mathf.Abs(sr.color.a - targetAlpha) > 0.01f)
        {
            Color c = sr.color;
            c.a = Mathf.MoveTowards(c.a, targetAlpha, fadeSpeed * Time.deltaTime);
            sr.color = c;
            yield return null;
        }

        Color final = sr.color;
        final.a = targetAlpha;
        sr.color = final;
    }
}