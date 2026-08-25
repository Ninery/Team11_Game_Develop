using UnityEngine;
using System.Collections;

public class MainMenuCat : MonoBehaviour, IClickable, IHoverable
{
    [Header("Setup")]
    public string nextSceneName = "Tutorial";
    public float delayBeforeLoad = 1f;
    public float runOffSpeed = 3f;

    private bool clicked = false;

    public void OnHoverEnter() { }
    public void OnHoverExit() { }

    public void OnClick()
    {
        if (clicked) return;
        clicked = true;

        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        float elapsed = 0f;
        while (elapsed < delayBeforeLoad)
        {
            transform.position += Vector3.right * runOffSpeed * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        SceneTransition.Instance.GoToScene(nextSceneName);
    }
}