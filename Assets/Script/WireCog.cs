using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WireCog : MonoBehaviour
{
    [Header("Setup")]
    public RectTransform needle;
    public float rotationSpeed = 180f;

    [Header("Safe Zone (degrees, 0 = right, clockwise)")]
    public float safeZoneStart = 80f;
    public float safeZoneEnd = 100f;
    public float clickTolerance = 5f;

    [Header("UI")]
    public GameObject alignPromptText;
    public GameObject blinkLight;
    public float blinkOnDuration = 0.2f;
    public float blinkOffDuration = 0.2f;

    [Header("Linked Wire (unsnaps on miss)")]
    public WireDrag linkedWire;

    public System.Action OnSolved;

    private bool isSpinning = false;
    private bool isSolved = false;
    private float currentAngle = 0f;
    private float blinkTimer = 0f;
    private bool blinkState = false;
    private bool wasInZone = false;

    void Update()
    {
        if (!isSpinning || isSolved) return;

        currentAngle += rotationSpeed * Time.deltaTime;
        currentAngle %= 360f;

        if (needle != null)
            needle.localRotation = Quaternion.Euler(0f, 0f, -currentAngle);

        bool inZone = IsInSafeZone();

        if (inZone)
        {
            blinkTimer += Time.deltaTime;
            float currentDuration = blinkState ? blinkOnDuration : blinkOffDuration;

            if (blinkTimer >= currentDuration)
            {
                blinkTimer = 0f;
                blinkState = !blinkState;
                if (blinkLight != null)
                    blinkLight.SetActive(blinkState);
            }
        }
        else if (wasInZone)
        {
            if (blinkLight != null)
                blinkLight.SetActive(false);
            blinkTimer = 0f;
            blinkState = false;
        }

        wasInZone = inZone;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleClick();
        }
    }

    public void StartSpinning()
    {
        isSpinning = true;

        if (alignPromptText != null)
            alignPromptText.SetActive(true);
    }

    private void HandleClick()
    {
        bool inZone = IsInSafeZone();

        if (inZone)
        {
            LockIn();
        }
        else
        {
            MissAndRetract();
        }
    }

    private bool IsInSafeZone()
    {
        return currentAngle >= safeZoneStart - clickTolerance &&
               currentAngle <= safeZoneEnd + clickTolerance;
    }

    private void LockIn()
    {
        isSolved = true;
        isSpinning = false;

        if (alignPromptText != null)
            alignPromptText.SetActive(false);

        if (blinkLight != null)
            blinkLight.SetActive(true);

        OnSolved?.Invoke();
    }

    private void MissAndRetract()
    {
        isSpinning = false;
        currentAngle = 0f;

        if (alignPromptText != null)
            alignPromptText.SetActive(false);

        if (blinkLight != null)
            blinkLight.SetActive(false);

        if (linkedWire != null)
            linkedWire.RetractToOrigin();
    }
}