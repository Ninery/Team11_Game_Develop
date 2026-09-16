using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    public static CatManager Instance;

    public int totalCats = 9;

    // Permanently collected cats
    private HashSet<string> collectedCatIds = new HashSet<string>();

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

    public void CollectCat(string catId)
    {
        if (string.IsNullOrEmpty(catId))
            return;

        // Only count the cat the first time it is collected
        if (collectedCatIds.Add(catId))
        {
            if (CatPopup.Instance != null)
            {
                CatPopup.Instance.ShowPopup(
                    collectedCatIds.Count,
                    totalCats
                );
            }
        }
    }

    public bool HasCollectedCat(string catId)
    {
        return !string.IsNullOrEmpty(catId) &&
               collectedCatIds.Contains(catId);
    }

    public int GetTotalCollected()
    {
        return collectedCatIds.Count;
    }

    // Kept for MonsterPatrol compatibility.
    // Collected cats are now permanent, so death does NOT reset them.
    public void ResetSessionProgress()
    {
        // Do nothing
    }

    // Kept in case other scripts still call it.
    // There is no separate session progress anymore.
    public void CommitSessionProgress()
    {
        // Do nothing
    }

    // Actually clears collected cats. Call this ONLY when the player is
    // starting a brand new playthrough from the very beginning (e.g. from
    // the pause menu's "Main Menu" button) -- never on a mid-level
    // checkpoint restart, which should keep whatever cats were already
    // collected.
    public void ResetAllProgress()
    {
        collectedCatIds.Clear();
    }
}