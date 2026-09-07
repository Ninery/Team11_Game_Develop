using UnityEngine;

public class EndingScreen : MonoBehaviour
{
    [Header("Ending 1 - Not All Cats Collected")]
    public GameObject ending1Canvas;

    [Header("Ending 2 - All Cats Collected")]
    public GameObject ending2Canvas;

    void Start()
    {
        bool gotAllCats = CatManager.Instance != null &&
                           CatManager.Instance.GetTotalCollected() >= CatManager.Instance.totalCats;

        if (ending1Canvas != null) ending1Canvas.SetActive(!gotAllCats);
        if (ending2Canvas != null) ending2Canvas.SetActive(gotAllCats);
    }

    public void RestartToMenu()
    {
        SceneTransition.Instance.GoToScene("StartMenu");
    }
}