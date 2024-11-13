using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform circleSpace;
    public RectTransform innerCircle;

    private Vector2 startPosition;
    private Vector2 inputVector;

    void Start()
    {
        SetupJoystick();
    }

    public void OnDrag(PointerEventData eventData)
    {
        CalculateInnerCirclePosition(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = eventData.position;
        CalculateInnerCirclePosition(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        innerCircle.anchoredPosition = Vector2.zero;
    }

    private void CalculateInnerCirclePosition(Vector2 position)
    {
        Vector2 directPosition = position - (Vector2)circleSpace.position;
        if (directPosition.magnitude > circleSpace.rect.width / 2f)
        {
            directPosition = directPosition.normalized * circleSpace.rect.width / 2f;
        }
        innerCircle.anchoredPosition = directPosition;
        inputVector = directPosition / (circleSpace.rect.width / 2f);
    }

    public float Horizontal()
    {
        return inputVector.x;
    }

    public float Vertical()
    {
        return inputVector.y;
    }

    public Vector2 GetInputVector()
    {
        return inputVector;
    }

    public bool IsJoystickActive()
    {
        return inputVector != Vector2.zero;
    }

    private void SetupJoystick()
    {
        // Create the circleSpace
        GameObject circleSpaceGO = new GameObject("CircleSpace");
        circleSpaceGO.transform.SetParent(transform);
        circleSpace = circleSpaceGO.AddComponent<RectTransform>();
        Image circleSpaceImage = circleSpaceGO.AddComponent<Image>();
        circleSpaceImage.color = new Color(0, 0, 0, 0.5f); // Semi-transparent black

        // Set size and position for circleSpace
        circleSpace.anchorMin = new Vector2(0.5f, 0.5f);
        circleSpace.anchorMax = new Vector2(0.5f, 0.5f);
        circleSpace.pivot = new Vector2(0.5f, 0.5f);
        circleSpace.sizeDelta = new Vector2(200, 200); // Width and Height
        circleSpace.anchoredPosition = Vector2.zero;

        // Create the innerCircle
        GameObject innerCircleGO = new GameObject("InnerCircle");
        innerCircleGO.transform.SetParent(circleSpace);
        innerCircle = innerCircleGO.AddComponent<RectTransform>();
        Image innerCircleImage = innerCircleGO.AddComponent<Image>();
        innerCircleImage.color = new Color(1, 1, 1, 0.5f); // Semi-transparent white

        // Set size and position for innerCircle
        innerCircle.anchorMin = new Vector2(0.5f, 0.5f);
        innerCircle.anchorMax = new Vector2(0.5f, 0.5f);
        innerCircle.pivot = new Vector2(0.5f, 0.5f);
        innerCircle.sizeDelta = new Vector2(100, 100); // Width and Height
        innerCircle.anchoredPosition = Vector2.zero;
    }
}
