using UnityEngine;

public class InfiniteScrollBackground : MonoBehaviour
{
    public float scrollSpeed = 0.25f;
    public Transform segmentA;
    public Transform segmentB;

    private float segmentWidth;

    void Start()
    {
        SpriteRenderer sr = segmentA.GetComponent<SpriteRenderer>();
        segmentWidth = sr.size.x * segmentA.lossyScale.x;

        segmentB.position = segmentA.position + new Vector3(segmentWidth, 0f, 0f);
    }

    void Update()
    {
        segmentA.position += Vector3.left * scrollSpeed * Time.deltaTime;
        segmentB.position += Vector3.left * scrollSpeed * Time.deltaTime;

        if (segmentA.position.x <= -segmentWidth)
            segmentA.position += new Vector3(segmentWidth * 2f, 0f, 0f);

        if (segmentB.position.x <= -segmentWidth)
            segmentB.position += new Vector3(segmentWidth * 2f, 0f, 0f);
    }
}