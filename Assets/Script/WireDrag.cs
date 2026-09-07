using UnityEngine;
using UnityEngine.EventSystems;

public class WireDrag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("Setup")]
    public RectTransform head;
    public RectTransform targetSocket;
    public float snapRange = 20f;

    private RectTransform rect;
    private RectTransform parentRect;
    private Vector2 originPos;
    private Vector2 restTip;
    private Vector2 currentTip;
    private bool isLocked = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        parentRect = rect.parent as RectTransform;

        originPos = rect.anchoredPosition;

        float restAngleRad = rect.localEulerAngles.z * Mathf.Deg2Rad;
        restTip = originPos + new Vector2(Mathf.Cos(restAngleRad), Mathf.Sin(restAngleRad)) * rect.sizeDelta.x;

        currentTip = restTip;
        UpdateVisual();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        Vector2 currentLocal;
        Vector2 previousLocal;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect, eventData.position, eventData.pressEventCamera, out currentLocal);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect, eventData.position - eventData.delta, eventData.pressEventCamera, out previousLocal);

        currentTip += currentLocal - previousLocal;

        UpdateVisual();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        float distance = Vector2.Distance(currentTip, targetSocket.anchoredPosition);

        if (distance <= snapRange)
        {
            SnapToSocket();
        }
        else
        {
            currentTip = restTip;
            UpdateVisual();
        }
    }

    private void SnapToSocket()
    {
        isLocked = true;
        currentTip = targetSocket.anchoredPosition;
        UpdateVisual();

        WireSocket socket = targetSocket.GetComponent<WireSocket>();
        if (socket != null)
            socket.OnPlugConnected();
    }

    public void RetractToOrigin()
    {
        isLocked = false;
        currentTip = restTip;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        Vector2 delta = currentTip - originPos;
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        float length = delta.magnitude;

        rect.localRotation = Quaternion.Euler(0f, 0f, angle);

        float scaleCompensation = rect.localScale.x != 0 ? rect.localScale.x : 1f;
        rect.sizeDelta = new Vector2(length / scaleCompensation, rect.sizeDelta.y);

        if (head != null)
        {
            head.anchoredPosition = currentTip;
            head.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}