using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxFX : MonoBehaviour
{
    public bool continuousMovement = false;
    public Vector2 continuousMovementSpeedFactor = new(0.0f, 0.0f);
    public Camera cam;
    public Material skyboxMat;
    [Range(0.0f, 1.0f)]
    public float speedFactor;
    private Vector2 continuousMovementAcceleration = new(0.0f, 0.0f);

    void Start()
    {
        if (!continuousMovement) RenderSettings.skybox = skyboxMat;

    }

    void Update()
    {
        if (!continuousMovement) skyboxMat.SetVector("_Offset", cam.transform.position * speedFactor);
        if (continuousMovement)
        {
            skyboxMat.SetVector("_Offset", new(cam.transform.position.x + continuousMovementAcceleration.x,
                                                                cam.transform.position.y + continuousMovementAcceleration.y,
                                                                0.0f, 0.0f));
            continuousMovementAcceleration += continuousMovementSpeedFactor;
        }
    }
}
