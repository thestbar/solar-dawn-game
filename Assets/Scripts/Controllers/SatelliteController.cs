using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SatelliteController : MonoBehaviour
{
    private GameObject targetObject;
    private TargetCircles targetCircles;
    private float innerRadius;
    private float middleRadius;
    private float outerRadius;
    private Transform myCamera;
    private Vector3 startCameraPosition;
    public float satelliteRotationSpeed = 2.0f;
    public string targetName = "PS_Satellite_Moon";
    public float score;
    public GameObject explosionObject;
    private Vector3 targetStartingPosition;
    private float maxDistanceFromEarth;

    // Print score on screen
    private TextMeshProUGUI scoreText;

    // Go to the end level screen
    private bool showPoints = false;
    private bool isWin = false;
    private bool destroyProbeActive = false;

    private GameObject soundFXObject;
    public AudioClip explosionSFX;
    public AudioClip scanTargetSFX;
    private AudioSource scanTargetSFXAudioSource;

    private void Start()
    {
        soundFXObject = GameObject.Find("SoundFX");
        targetObject = GameObject.Find(targetName);
        targetCircles = targetObject.GetComponent<TargetCircles>();
        innerRadius = targetCircles.innerRadius;
        middleRadius = targetCircles.middleRadius;
        outerRadius = targetCircles.outerRadius;
        myCamera = Camera.main.transform;
        startCameraPosition = myCamera.position;
        score = 0f;
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        scoreText.SetText("0.00 KB");
        maxDistanceFromEarth = targetCircles.GetMaxDistanceFromEarth();
        Debug.Log(maxDistanceFromEarth);
    }

    private void FixedUpdate()
    {
        if (gameObject.GetComponent<Rigidbody>().worldCenterOfMass.sqrMagnitude > maxDistanceFromEarth && !isWin)
        {
            Destroy(gameObject);
            Destroy(GameObject.FindGameObjectWithTag("Rocket"));
            PlayDestroySFX();
            ResetTargetPosition();
            myCamera.position = startCameraPosition;
            Camera.main.orthographicSize = 5.0f;
            // Let the player play again
            GameObject.Find("PS_Planet_Earth").GetComponent<Controller>().canPlay = true;
            return;
        }
        float distance = (transform.position - targetObject.transform.position).sqrMagnitude;
        myCamera.position = gameObject.transform.position + new Vector3(0, 0, -10);
        if (Camera.main.orthographicSize > 2.0f) Camera.main.orthographicSize -= 0.02f;
        if(!destroyProbeActive)
        {
            if (distance <= Mathf.Pow(innerRadius, 2))
            {
                Time.timeScale = 0.6f; // 0.6 - 0.7 - 0.8
                score += Random.Range(198f, 202f);
            }
            else if (distance <= Mathf.Pow(middleRadius, 2))
            {
                Time.timeScale = 0.7f;
                score += Random.Range(48f, 52f);
            }
            else if (distance <= Mathf.Pow(outerRadius, 2))
            {
                Time.timeScale = 0.8f;
                score += Random.Range(10f, 12f);
                scanTargetSFXAudioSource = PlayTargetScanSFX();
                showPoints = true;
            }
            else
            {
                Time.timeScale = 1f;
            }
        } else
        {
            score = 0;
        }

        // Probe should rotate a little bit
        string text = "";
        if (score < 1000)
        {
            text = score.ToString("0.00") + " KB";
        }
        else
        {
            text = (score / 1000f).ToString("0.00") + " MB";
        }

        gameObject.transform.Rotate(satelliteRotationSpeed, satelliteRotationSpeed, satelliteRotationSpeed, Space.Self);
        scoreText.SetText(text);

        if (distance > Mathf.Pow(outerRadius, 2) && showPoints)
        {
            Time.timeScale = 1f;
            isWin = true;
            StopTargetScanSFX(scanTargetSFXAudioSource);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Rocket")
        {
            return;
        }
        else
        {
            StartCoroutine(DestroyProbe());
        }
    }


    private void ProbeExplosion(Vector3 position)
    {
        Instantiate(explosionObject, position, Quaternion.identity);
    }

    private void ResetTargetPosition()
    {
        targetObject.transform.position = targetObject.GetComponent<CelestialBody>().GetStartingPosition();
    }

    private IEnumerator DestroyProbe()
    {
        destroyProbeActive = true;
        PlayDestroySFX();
        StopTargetScanSFX(scanTargetSFXAudioSource);
        Vector3 position = gameObject.transform.position;
        gameObject.GetComponent<ProjectileFX>().enabled = false;
        gameObject.GetComponent<LineRenderer>().enabled = false;
        gameObject.GetComponent<GravityForce>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().velocity = new(0f, 0f, 0f);
        ProbeExplosion(position);
        yield return new WaitForSeconds(1f);
        ResetTargetPosition();
        Time.timeScale = 1f;
        // Wait for some seconds to see the explosion!!!
        myCamera.position = startCameraPosition;
        Camera.main.orthographicSize = 5f;
        score = 0;
        scoreText.SetText("0 KB");
        destroyProbeActive = false;
        Destroy(GameObject.FindGameObjectWithTag("Rocket"));
        GameObject.Find("PS_Planet_Earth").GetComponent<Controller>().canPlay = true;
        Destroy(gameObject);
    }

    void PlayDestroySFX()
    {
        AudioSource audioSource = soundFXObject.AddComponent<AudioSource>();
        audioSource.volume = SoundManager.getVolume();
        audioSource.PlayOneShot(explosionSFX);
    }

    AudioSource PlayTargetScanSFX()
    {
        if (showPoints) return scanTargetSFXAudioSource;
        AudioSource audioSource = soundFXObject.AddComponent<AudioSource>();
        audioSource.clip = scanTargetSFX;
        audioSource.loop = true;
        audioSource.volume = SoundManager.getVolume();
        audioSource.Play();
        return audioSource;
    }

    void StopTargetScanSFX(AudioSource audioSource)
    {
        if (audioSource == null) return;
        Destroy(audioSource);
    }

    public float GetScore()
    {
        return score;
    }

    public bool IsWin()
    {
        return isWin;
    }
}
