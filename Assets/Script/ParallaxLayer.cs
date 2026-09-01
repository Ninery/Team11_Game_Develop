using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public Transform cameraTransform;
    [Range(0f, 1f)] public float parallaxFactor = 0.5f;

    private Vector3 startPosition;
    private Vector3 startCameraPosition;

    void Start()
    {
        startPosition = transform.position;
        startCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 totalDelta = cameraTransform.position - startCameraPosition;
        transform.position = startPosition + new Vector3(totalDelta.x * parallaxFactor, totalDelta.y * parallaxFactor, 0f);
    }
}