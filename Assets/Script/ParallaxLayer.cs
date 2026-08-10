using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public Transform cameraTransform;
    [Range(0f, 1f)] public float parallaxFactor = 0.5f; // 0 = static, 1 = moves with camera

    private Vector3 lastCameraPosition;

    void Start()
    {
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(delta.x * parallaxFactor, delta.y * parallaxFactor, 0);
        lastCameraPosition = cameraTransform.position;
    }
}