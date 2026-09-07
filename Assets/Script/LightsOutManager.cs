using UnityEngine;

public class LightsOutManager : MonoBehaviour
{
    public static LightsOutManager Instance;
    public const int GridSize = 9; // 3x3, index layout: 0 1 2 / 3 4 5 / 6 7 8

    [Header("Starting Pattern (which buttons begin lit)")]
    public bool[] startingPattern = new bool[GridSize];

    [HideInInspector] public bool[] buttonStates = new bool[GridSize];
    [HideInInspector] public bool doorOpen = false;

    public System.Action OnStateChanged;
    public System.Action OnSolved;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            for (int i = 0; i < GridSize; i++)
                buttonStates[i] = startingPattern[i];

            doorOpen = CheckSolved();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleButton(int index)
    {
        if (doorOpen) return;

        ToggleAt(index);
        int row = index / 3;
        int col = index % 3;

        if (row > 0) ToggleAt(index - 3);
        if (row < 2) ToggleAt(index + 3);
        if (col > 0) ToggleAt(index - 1);
        if (col < 2) ToggleAt(index + 1);

        OnStateChanged?.Invoke();

        if (CheckSolved())
        {
            doorOpen = true;
            OnSolved?.Invoke();
        }
    }

    private void ToggleAt(int index) => buttonStates[index] = !buttonStates[index];

    private bool CheckSolved()
    {
        foreach (bool state in buttonStates)
            if (!state) return false;
        return true;
    }
<<<<<<< Updated upstream
=======

    public void ResetToStartingPattern()
    {
        if (doorOpen) return;

        for (int i = 0; i < GridSize; i++)
            buttonStates[i] = startingPattern[i];

        OnStateChanged?.Invoke();
    }
>>>>>>> Stashed changes
}