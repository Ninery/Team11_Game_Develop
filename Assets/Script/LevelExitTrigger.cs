using UnityEngine;

public class LevelExitTrigger : MonoBehaviour
{
    public string nextSceneName;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CatManager.Instance.CommitSessionProgress();
            SceneTransition.Instance.GoToScene(nextSceneName);
        }
    }
}