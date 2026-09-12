using UnityEngine;

public class NewMusic : MonoBehaviour
{
    public AudioClip newMusic;

    void Start()
    {
        if (GlobalBGM.Instance != null)
        {
            GlobalBGM.Instance.ChangeMusic(newMusic);
        }
    }
}