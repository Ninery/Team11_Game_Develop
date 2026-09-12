using UnityEngine;

public class PigeonAudio : MonoBehaviour
{
    [Header("Eating Sound")]
    public AudioSource eatingAudio;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (eatingAudio == null || mainCamera == null)
            return;

        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        bool visible =
            viewportPos.x >= 0f &&
            viewportPos.x <= 1f &&
            viewportPos.y >= 0f &&
            viewportPos.y <= 1f &&
            viewportPos.z > 0f;

        if (!visible)
        {
            eatingAudio.Stop();
        }
    }

    public void PlayPigeonEatingSound()
    {
        if (eatingAudio != null && eatingAudio.clip != null)
        {
            eatingAudio.Play();
        }
    }
}