using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class FlashlightStunEffect : MonoBehaviour
{
    public Light2D flashlightLight;

    [Header("Flash")]
    public float normalIntensity = 1f;
    public float flashIntensity = 2f;
    public float flashDuration = 0.5f;

    [Header("Flicker (cooldown)")]
    public float flickerDuration = 6f;
    public float flickerMinIntensity = 0.2f;
    public float flickerMaxIntensity = 0.6f;
    public float flickerInterval = 0.15f;

    private bool isOnCooldown = false;

    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }

    public void PlayStunFlashEffect()
    {
        StartCoroutine(FlashThenFlickerSequence());
    }

    private IEnumerator FlashThenFlickerSequence()
    {
        isOnCooldown = true;

        if (flashlightLight != null)
            flashlightLight.intensity = flashIntensity;

        yield return new WaitForSeconds(flashDuration);

        float elapsed = 0f;
        while (elapsed < flickerDuration)
        {
            if (flashlightLight != null)
                flashlightLight.intensity = Random.Range(flickerMinIntensity, flickerMaxIntensity);

            yield return new WaitForSeconds(flickerInterval);
            elapsed += flickerInterval;
        }

        if (flashlightLight != null)
            flashlightLight.intensity = normalIntensity;

        isOnCooldown = false;
    }
}