using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetPointer : MonoBehaviour
{
    public float widthOffset = 30;
    public float heightOffset = 50;
    // How many degrees before and after the corner the
    // indicator will remain to the corner
    public float angleOffset = 1.5f;
    private Canvas canvas;
    private float canvasMaxWidth;
    private float canvasMaxHeight;
    private Vector2 pointerPosition;
    private GameObject targetObject;
    private GameObject pointerObject;
    private Image pointerObjectImage;
    private RectTransform pointerRectTranform;
    private PauseButton pauseButton;

    void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        RectTransform objectRectTransform = canvas.GetComponent<RectTransform>();
        canvasMaxWidth = objectRectTransform.rect.width;
        canvasMaxHeight = objectRectTransform.rect.height;
        targetObject = GameObject.Find("PS_Satellite_Moon");
        pointerObject = GameObject.Find("Pointer");
        pointerObjectImage = pointerObject.GetComponent<Image>();
        pointerRectTranform = transform.Find("Pointer").GetComponent<RectTransform>();
        pauseButton = GameObject.Find("Button Pause").GetComponent<PauseButton>();
    }

    void Update()
    {
        GameObject probe = GameObject.Find("ProbeGameObject(Clone)");
        SatelliteController satelliteController = null;
        if (probe != null)
        {
            satelliteController = probe.GetComponent<SatelliteController>();
        }
        var color = pointerObjectImage.color;
        if (pauseButton.isGamePaused || (satelliteController != null && satelliteController.IsWin()))
        {
            color.a = 0.05f;
        }
        else
        {
            color.a = 1f;   
        }
        pointerObjectImage.color = color;
        // Each frame get the size of the canvas
        // In case of screen scaling.
        RectTransform objectRectTransform = canvas.GetComponent<RectTransform>();
        canvasMaxWidth = objectRectTransform.rect.width;
        canvasMaxHeight = objectRectTransform.rect.height;
        Vector3 targetPosition = targetObject.transform.position;
        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = GetAngleFromVectorFloat(dir);
        pointerRectTranform.localEulerAngles = new Vector3(0, 0, angle);
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffScreen = targetPositionScreenPoint.x <= 0 ||
            targetPositionScreenPoint.x >= Screen.width ||
            targetPositionScreenPoint.y <= 0 ||
            targetPositionScreenPoint.y >= Screen.height;
        if (isOffScreen)
        {
            if (!pointerObjectImage.enabled)
            {
                pointerObjectImage.enabled = true;
            }
            if (angle <= 45 - angleOffset)
            {
                pointerPosition.x = canvasMaxWidth / 2 - widthOffset;
                pointerPosition.y = canvasMaxHeight * angle / 90;
            }
            else if (angle >= 45 - angleOffset && angle <= 45 + angleOffset)
            {
                pointerPosition.x = canvasMaxWidth / 2 - widthOffset;
                pointerPosition.y = canvasMaxHeight / 2 - heightOffset;
            }
            else if (angle <= 90)
            {
                pointerPosition.x = canvasMaxWidth / 2 * (2 - angle / 45);
                pointerPosition.y = canvasMaxHeight / 2 - heightOffset;
            }
            else if (angle <= 135 - angleOffset)
            {
                pointerPosition.x = (90 - angle) * canvasMaxWidth / 90;
                pointerPosition.y = canvasMaxHeight / 2 - heightOffset;
            }
            else if (angle >= 135 - angleOffset && angle <= 135 + angleOffset)
            {
                pointerPosition.x = -canvasMaxWidth / 2 + widthOffset;
                pointerPosition.y = canvasMaxHeight / 2 - heightOffset;
            }
            else if (angle <= 180)
            {
                pointerPosition.x = -canvasMaxWidth / 2 + widthOffset;
                pointerPosition.y = canvasMaxHeight / 2 * (4 - angle / 45);
            }
            else if (angle <= 225 - angleOffset)
            {
                pointerPosition.x = -canvasMaxWidth / 2 + widthOffset;
                pointerPosition.y = canvasMaxHeight * (180 - angle) / 90;
            }
            else if (angle >= 225 - angleOffset && angle <= 225 + angleOffset)
            {
                pointerPosition.x = -canvasMaxWidth / 2 + widthOffset;
                pointerPosition.y = -canvasMaxHeight / 2 + heightOffset;
            }
            else if (angle <= 270)
            {
                pointerPosition.x = canvasMaxWidth * (angle / 90 - 3);
                pointerPosition.y = -canvasMaxHeight / 2 + heightOffset;
            }
            else if (angle <= 315 - angleOffset)
            {
                pointerPosition.x = canvasMaxWidth * (angle - 270) / 90;
                pointerPosition.y = -canvasMaxHeight / 2 + heightOffset;
            }
            else if (angle >= 315 - angleOffset && angle <= 315 + angleOffset)
            {
                pointerPosition.x = canvasMaxWidth / 2 - widthOffset;
                pointerPosition.y = -canvasMaxHeight / 2 + heightOffset;
            }
            else
            {
                pointerPosition.x = canvasMaxWidth / 2 - widthOffset;
                pointerPosition.y = canvasMaxHeight * (angle / 90 - 4);
            }
            pointerRectTranform.transform.localPosition = new Vector3(pointerPosition.x, pointerPosition.y, 0);
        }
        else
        {
            if (pointerObjectImage.enabled)
            {
                pointerObjectImage.enabled = false;
            }
        }
    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
