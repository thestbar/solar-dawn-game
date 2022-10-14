using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Controller : MonoBehaviour
{
    private Vector3 clickDown = Vector3.zero;
    private Vector3 clickUp = Vector3.zero;
    private bool isClickDown = false;
    private Outline outline;

    [ColorUsage(true,true)]
    public Color hoverColor;

    [ColorUsage(true, true)]
    public Color clickColor;

    [Range(0.5f,1.5f)]
    public float maxTargetHelperScale;

    public GameObject rocket;
    public float instantiateFactor = 0.55f;
    public bool canPlay = false;
    public GameObject soundFXObject;
    public AudioClip soundFxRocketLaunch;
    public AudioClip clickedEarthSoundFX;
    public AudioClip hoveredEarthSoundFX;
    private AudioSource clickedEarthAudioSource = null;
    public Vector3 debugPos;
    private float framePower = 0f;
    private float speedFactor = 0.005f;
    public Camera cam;
    public LineRenderer lineRenderer;
    private const float ARROW_SCALE_1 = 0.33578f;
    private const float ARROW_SCALE_2 = 0.68057f;
    private const float ARROW_SCALE_3 = 1.02504f;
    private const float ARROW_SCALE_4 = 1.37033f;
    private const float ARROW_SCALE_5 = 1.71512f;

    void Start()
    {
        DisableTargetHelper();
        outline = gameObject.GetComponent<Outline>();
    }

    void Update()
    {
        if (!canPlay) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.transform.name == "PS_Planet_Earth" && outline.isActiveAndEnabled == false)
        {
            if (!outline.enabled) PlayHoveredEarthSoundFX();
            outline.OutlineColor = hoverColor;
            outline.OutlineWidth = 2f;
            outline.enabled = true;
        }
        if(!(Physics.Raycast(ray, out hit) && hit.transform.name == "PS_Planet_Earth") && outline.isActiveAndEnabled == true)
        {
            outline.enabled = false;
        }
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit) && hit.transform.name == "PS_Planet_Earth")
        {
            isClickDown = true;
            clickDown = Input.mousePosition;
            outline.OutlineColor = clickColor;
            outline.OutlineWidth = 2f;
            clickedEarthAudioSource = PlayClickedEarthSoundFX();
        }
        if (Input.GetMouseButtonUp(0) && isClickDown)
        {
            Destroy(clickedEarthAudioSource);
            isClickDown = false;
            clickUp = Input.mousePosition;
            DisableTargetHelper();
            outline.OutlineColor = hoverColor;
            outline.OutlineWidth = 2f;
            float mag1 = Vector3.Magnitude(clickDown - clickUp);
            if (mag1 < 1f) return;
            LaunchRocket(clickDown, clickUp);
            canPlay = false;
        }
        if (isClickDown)
        {
            outline.enabled = true;
            DrawTargetHelper(clickDown);
        }
    }

    void DrawTargetHelper(Vector3 startPos)
    {
        Vector3 endPos = Input.mousePosition;
        Vector3 dir = (startPos - endPos).normalized;
        float sqrMgn = (startPos - endPos).sqrMagnitude;
        float scale = sqrMgn * 0.00001f;
        if (scale > maxTargetHelperScale) scale = maxTargetHelperScale;
        framePower = (scale + 0.5f) * 263.157894737f;
        lineRenderer.enabled = true;
        float fakeScale;
        if (scale < ARROW_SCALE_1) fakeScale = ARROW_SCALE_1;
        else if (scale < ARROW_SCALE_2) fakeScale = ARROW_SCALE_2;
        else if (scale < ARROW_SCALE_3) fakeScale = ARROW_SCALE_3;
        else if (scale < ARROW_SCALE_4) fakeScale = ARROW_SCALE_4;
        else fakeScale = ARROW_SCALE_5;
        lineRenderer.SetPosition(0, dir);
        lineRenderer.SetPosition(1, dir + (dir * fakeScale));
    }

    void DisableTargetHelper()
    {
        lineRenderer.enabled = false;
    }

    void LaunchRocket(Vector3 startPoint, Vector3 endPoint)
    {
        // In case the user just clicks on earth do nothing.
        if (startPoint == endPoint) return;
        // In case the user gave no power do nothing.
        if (Mathf.Floor(framePower) == 0f) return;
        Vector3 direction = (startPoint - endPoint).normalized;
        Rigidbody clone;
        Vector3 startingPosition = new Vector3(direction.x * instantiateFactor, direction.y * instantiateFactor, 0f);
        clone = Instantiate(rocket.GetComponent<Rigidbody>(), startingPosition, Quaternion.identity);
        // Calculate the angle
        float startingAngle = Mathf.Rad2Deg * Mathf.Atan2((startPoint - endPoint).y, (startPoint - endPoint).x);
        clone.transform.Rotate(-startingAngle, 90f, 0f, Space.Self);
        clone.AddForce(direction * framePower * speedFactor, ForceMode.Impulse);
        // Play SoundFX
        AudioSource soundFXAudioSource = soundFXObject.AddComponent<AudioSource>();
        soundFXAudioSource.volume = SoundManager.getVolume();
        soundFXAudioSource.PlayOneShot(soundFxRocketLaunch);
    }


    AudioSource PlayClickedEarthSoundFX()
    {
        AudioSource audioSource = soundFXObject.AddComponent<AudioSource>();
        audioSource.volume = SoundManager.getVolume();
        audioSource.clip = clickedEarthSoundFX;
        audioSource.loop = true;
        audioSource.Play();
        return audioSource;
    }

    void PlayHoveredEarthSoundFX()
    {
        AudioSource audioSource = soundFXObject.AddComponent<AudioSource>();
        audioSource.volume = SoundManager.getVolume();
        audioSource.PlayOneShot(hoveredEarthSoundFX);
    }
}
