using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneEntryCheckpoint : MonoBehaviour
{
    private static HashSet<string> shownScenes = new HashSet<string>();

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (shownScenes.Contains(currentScene)) return;

        shownScenes.Add(currentScene);
        CheckpointManager.Instance.ShowSceneEntryPopup();
    }
}