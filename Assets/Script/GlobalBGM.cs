using UnityEngine;

public class GlobalBGM : MonoBehaviour
{
    public static GlobalBGM Instance;

    public AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeMusic(AudioClip newClip)
    {
        if (audioSource.clip == newClip)
            return;

        audioSource.clip = newClip;
        audioSource.Play();
    }
}