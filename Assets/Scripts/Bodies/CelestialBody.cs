using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public bool isRotatingAroundItsAxis = true;
    public float rotAngVelocity = 90.0f;
    public bool isOrbiting = true;
    public CelestialBody primaryCelestialBody;
    public float orbitRadius = 50.0f;
    public float orbitSpeed = 90.0f;
    public bool isGravPuller = false;
    public float mass = 100.0f;
    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = gameObject.transform.position;
        if (isOrbiting && primaryCelestialBody)
        {
            Vector3 newPosition = new Vector3(orbitRadius, 0f, 0f);
            transform.position = primaryCelestialBody.transform.position + newPosition;
        }
    }

    void FixedUpdate()
    {
        if (isRotatingAroundItsAxis)
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotAngVelocity);
        }

        if (isOrbiting && primaryCelestialBody)
        {
            transform.RotateAround(primaryCelestialBody.transform.position, Vector3.forward, Time.deltaTime * orbitSpeed);
        }
    }

    public Vector3 GetStartingPosition()
    {
        return startingPosition;
    }
}
