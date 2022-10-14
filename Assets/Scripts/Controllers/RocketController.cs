using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    private TrailRenderer fireTrail;
    private Rigidbody rb;
    private bool hasStopped = false;
    public GameObject probe;
    public GameObject rocketLineFol;
    public GameObject explosionObject;
    public Material trailMat;

    void Start()
    {
        fireTrail = gameObject.GetComponent<TrailRenderer>();
        fireTrail.material = trailMat;
        rb = gameObject.GetComponent<Rigidbody>();
        Instantiate(rocketLineFol);
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the length of the trail renderer
        // trail renderer is the fire effect
        float time = -0.22f * rb.velocity.magnitude + 0.80f;
        fireTrail.time = time;
    }

    void FixedUpdate()
    {
        float distFromEarth = gameObject.transform.position.sqrMagnitude;
        if (distFromEarth > 3.0f && !hasStopped)
        {
            Vector3 velocity = rb.velocity;
            fireTrail.enabled = false;
            float randomFloat = Random.Range(0f, 1.0f);
            Vector3 randomVector = new Vector3(randomFloat, Mathf.Sqrt(1 - randomFloat * randomFloat), 0f);
            rb.velocity = -rb.velocity * 0.1f + randomVector * 0.001f;
            hasStopped = true;
            InstantiateProbe(velocity);
        }
    }

    private void InstantiateProbe(Vector3 initialVelocity)
    {
        Rigidbody voyagerRb = probe.GetComponent<Rigidbody>();
        gameObject.GetComponent<Collider>().enabled = false;
        voyagerRb = Instantiate(voyagerRb, gameObject.transform.position, Quaternion.identity);
        voyagerRb.AddForce(initialVelocity, ForceMode.VelocityChange);
    }

    private void RocketExplosion(Vector3 position)
    {
        Instantiate(explosionObject, position, Quaternion.identity);
    }

    public void DestroyRocket()
    {
        Vector3 position = gameObject.transform.position;
        Destroy(gameObject);
        RocketExplosion(position);
        GameObject.Find("PS_Planet_Earth").GetComponent<Controller>().canPlay = true;
    }
}
