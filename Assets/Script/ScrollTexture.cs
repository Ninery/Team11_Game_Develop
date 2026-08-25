using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public float scrollSpeed = 0.05f;
    private Renderer rend;
    private Vector2 offset;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        offset.x += scrollSpeed * Time.deltaTime;

        if (rend.material.HasProperty("_MainTex"))
            rend.material.SetTextureOffset("_MainTex", offset);

        if (rend.material.HasProperty("_BaseMap"))
            rend.material.SetTextureOffset("_BaseMap", offset);
    }
}