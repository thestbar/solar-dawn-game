using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFX : MonoBehaviour
{
    public string celestialBodyName;
    public Material lineRendererMat;
    private GameObject celestialBody;
    private Transform targetTransform;
    private LineRenderer projLineRenderer;
    private float outerCircleRadius;
    private float distance;

    void Start()
    {
        celestialBody = GameObject.Find(celestialBodyName);
        outerCircleRadius = celestialBody.GetComponent<TargetCircles>().outerRadius;
        targetTransform = celestialBody.transform;
        projLineRenderer = gameObject.GetComponent<LineRenderer>();
        projLineRenderer.material = lineRendererMat;
        projLineRenderer.enabled = false;
        projLineRenderer.SetPosition(0, gameObject.transform.position);
        projLineRenderer.SetPosition(1, targetTransform.position);
    }

    void Update()
    {
        distance = (gameObject.transform.position - targetTransform.position).sqrMagnitude;
        if (distance <= Mathf.Pow(outerCircleRadius, 2) && projLineRenderer.enabled == false)
        {
            projLineRenderer.enabled = true;
        }
        if (distance > Mathf.Pow(outerCircleRadius, 2) && projLineRenderer.enabled == true)
        {
            projLineRenderer.enabled = false;
        } 
        if (projLineRenderer.enabled == true)
        {
            projLineRenderer.SetPosition(0, gameObject.transform.position);
            projLineRenderer.SetPosition(1, targetTransform.position);
        }
    }
}
