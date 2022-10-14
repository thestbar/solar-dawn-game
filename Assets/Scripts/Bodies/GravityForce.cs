using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityForce : MonoBehaviour
{
    public float mass;
    public float GravitationalConstant = 0.001f;
    private CelestialBody[] celestialBodies;
    private Rigidbody rb;

    // Calculating rotation
    private float previousAngle;

    private void Awake()
    {
        celestialBodies = FindObjectsOfType<CelestialBody>();
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        // Calculating starting slope and angle
        previousAngle = Mathf.Rad2Deg * Mathf.Atan2(
            gameObject.transform.position.y, gameObject.transform.position.x);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (CelestialBody cb in celestialBodies)
        {
            if (!cb.isGravPuller) continue;
            float distance = (cb.transform.position -
                transform.position).magnitude;
            Vector3 direction = (cb.transform.position -
                transform.position).normalized;
            Vector3 force = (GravitationalConstant * cb.mass /
                Mathf.Pow(distance, 2)) * direction;
            // actually it is the acceleration (force/mass)
            rb.AddForce(force, ForceMode.Acceleration);
            // Depending the force rotate the rocket a bit
            // Think the fact that on the next levels there 
            // will be more than one bodies that will pull the object
        }
        float currentAngle = Mathf.Rad2Deg * Mathf.Atan2(
            transform.position.y, transform.position.x);
        transform.Rotate(2f * (previousAngle - currentAngle),
            0f, 0f, Space.Self);
        previousAngle = currentAngle;
    }
}
