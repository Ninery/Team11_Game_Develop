using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [Header("Cursor Objects")]
    public RectTransform normalCursor;
    public RectTransform clickCursor;
    public RectTransform pawCursor;

    private GameObject hoveredObject;
    private RectTransform activeCursor;

    void Start()
    {
        Cursor.visible = false;
        DontDestroyOnLoad(gameObject);
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Cursor.visible = false;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SetActiveCursor(normalCursor);
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        if (activeCursor != null)
            activeCursor.position = mousePos;

        GameObject newHover = FindHoveredObject(mousePos);

        if (newHover != hoveredObject)
        {
            if (hoveredObject != null)
            {
                IHoverable oldHoverable = hoveredObject.GetComponentInParent<IHoverable>();
                if (oldHoverable != null) oldHoverable.OnHoverExit();
            }

            if (newHover != null)
            {
                IHoverable newHoverable = newHover.GetComponentInParent<IHoverable>();
                if (newHoverable != null) newHoverable.OnHoverEnter();
            }

            hoveredObject = newHover;
            UpdateCursorVisual();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && hoveredObject != null)
        {
            IClickable clickable = hoveredObject.GetComponentInParent<IClickable>();
            if (clickable != null) clickable.OnClick();
        }
    }

    private void UpdateCursorVisual()
    {
        if (hoveredObject == null)
        {
            SetActiveCursor(normalCursor);
            return;
        }

        CursorHoverTarget target = hoveredObject.GetComponentInParent<CursorHoverTarget>();
        CursorType type = target != null ? target.hoverCursorType : CursorType.Click;

        switch (type)
        {
            case CursorType.Paw:
                SetActiveCursor(pawCursor);
                break;
            default:
                SetActiveCursor(clickCursor);
                break;
        }
    }

    private void SetActiveCursor(RectTransform newCursor)
    {
        if (activeCursor == newCursor) return;

        Vector2 lastPos = activeCursor != null ? (Vector2)activeCursor.position : Vector2.zero;

        if (normalCursor != null) normalCursor.gameObject.SetActive(false);
        if (clickCursor != null) clickCursor.gameObject.SetActive(false);
        if (pawCursor != null) pawCursor.gameObject.SetActive(false);

        activeCursor = newCursor;
        if (activeCursor != null)
        {
            activeCursor.gameObject.SetActive(true);
            activeCursor.position = lastPos;
        }
    }

    private GameObject FindHoveredObject(Vector2 screenPos)
    {
        if (EventSystem.current != null)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = screenPos;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                if (result.gameObject.GetComponentInParent<CursorHoverTarget>() != null)
                    return result.gameObject;
            }
        }

        Camera cam = Camera.main;
        if (cam != null)
        {
            Vector2 worldPos = cam.ScreenToWorldPoint(screenPos);
            Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);

            foreach (var hit in hits)
            {
                if (hit.GetComponent<CursorHoverTarget>() != null)
                    return hit.gameObject;
            }
        }

        return null;
    }
}