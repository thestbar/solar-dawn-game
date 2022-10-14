using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLineRendererHandler : MonoBehaviour
{
    GameObject probe;
    LineRenderer probeLineRenderer;
    public float lineRendererThicknessAtTarget = 0.5f;
    
    void Update()
    {
        probe = GameObject.FindGameObjectWithTag("Probe");
        if (probe == null) return;
        probeLineRenderer = probe.GetComponent<LineRenderer>();
        if (probeLineRenderer == null) return;
        if (probeLineRenderer.endWidth == lineRendererThicknessAtTarget) return;
        probeLineRenderer.endWidth = lineRendererThicknessAtTarget;
    }
}
