using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHelper : MonoBehaviour
{
    private float width;
    private LineRenderer lineRenderer;
    private float magnitude;
    private float multiplier = 1000f;
    private float rate = 1.0001f;

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    void Update()
    {
        width = lineRenderer.startWidth;
        lineRenderer.material.mainTextureScale = new Vector2(1f / width, 1.0f);
        magnitude = (lineRenderer.GetPosition(0) - lineRenderer.GetPosition(1)).sqrMagnitude * multiplier;
        if(magnitude < 95f)
        {
            Vector3 pos = lineRenderer.GetPosition(1);
            lineRenderer.SetPosition(1, pos * rate);
        }
        else if(magnitude == 95f)
        {
            return;
        }
        else if (magnitude < 400f)
        {
            Vector3 pos = lineRenderer.GetPosition(1);
            lineRenderer.SetPosition(1, pos * rate);
        }
        else if (magnitude == 400f)
        {
            return;
        }
        else if (magnitude < 881f)
        {
            Vector3 pos = lineRenderer.GetPosition(1);
            lineRenderer.SetPosition(1, pos * rate);
        }
        else if (magnitude == 881f)
        {
            return;
        }
        else if (magnitude < 1599f)
        {
            Vector3 pos = lineRenderer.GetPosition(1);
            lineRenderer.SetPosition(1, pos * rate);
        }
        else if (magnitude == 1599f)
        {
            return;
        }
        else
        {
            return;
        }
    }
}
