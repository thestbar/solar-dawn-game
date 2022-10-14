using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLineFollower : MonoBehaviour
{
    private GameObject rocket;
    private GameObject probe;
    private bool isProbeDead = false;
    public Material mat;
    private TrailRenderer trailRenderer;
    private float width;

    private void OnEnable()
    {
        trailRenderer = gameObject.GetComponent<TrailRenderer>();
        trailRenderer.material = mat;
    }

    private void Awake()
    {
        trailRenderer = gameObject.GetComponent<TrailRenderer>();
        trailRenderer.material = mat;
    }

    void Start()
    {
        rocket = GameObject.FindGameObjectWithTag("Rocket");
    }

    void FixedUpdate()
    {
        if (isProbeDead) return;
        try
        {
            probe = GameObject.FindGameObjectWithTag("Probe");
        }
        catch
        {
            probe = null;
        }
        try
        {
            if (probe == null)
            {
                gameObject.transform.position = rocket.transform.position;
            }
            else
            {
                gameObject.transform.position = probe.transform.position;
            }
        }
        catch
        {
            isProbeDead = true;
        }
        width = trailRenderer.startWidth;
        trailRenderer.material.mainTextureScale = new Vector2(1f / width, 1.0f);
    }
}
