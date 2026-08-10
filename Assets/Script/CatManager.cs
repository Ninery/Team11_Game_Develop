using UnityEngine;

public class CatManager : MonoBehaviour
{
    public static CatManager Instance;

    public int totalCats = 9;

    private int committedCats = 0; // locked in, survives death
    private int sessionCats = 0;   // collected in current scene, wiped on death

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectCat()
    {
        sessionCats++;
        CatPopup.Instance.ShowPopup(committedCats + sessionCats, totalCats);
    }

    // call this right before loading the NEXT scene (successful progression)
    public void CommitSessionProgress()
    {
        committedCats += sessionCats;
        sessionCats = 0;
    }

    // call this right before reloading the CURRENT scene (player died)
    public void ResetSessionProgress()
    {
        sessionCats = 0;
    }

    public int GetTotalCollected()
    {
        return committedCats + sessionCats;
    }
}