using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameIntro : MonoBehaviour
{
    public Camera sceneCamera;
    public GameObject firstIntroTextObject;
    public GameObject firstIntroTextChildrenObject;

    public GameObject secondIntroTextObject;
    public GameObject secondIntroTextChildrenObject;

    public GameObject thirdIntroTextObject;
    public GameObject thirdIntroTextChildrenObject;

    public AudioClip lettersSoundFX;
    public AudioClip moveCameraSoundFX;

    public GameObject pauseButtonObject;
    public GameObject earthButtonObject;
    public GameObject moonButtonObject;

    Button pauseButton;
    Button earthButton;
    Button moonButton;

    ColorBlock pauseButtonColor;
    ColorBlock earthButtonColor;
    ColorBlock moonButtonColor;

    char[] firstIntroSentence = "This is home, Earth!".ToCharArray();
    char[] secondIntroSentence = "This is target, Moon!".ToCharArray();
    char[] thirdIntroSentence = "Use mouse wheel to zoom in and out.".ToCharArray();
    char[] fourthIntroSentence = "Drag to Aim!".ToCharArray();

    int index = 1;

    float currentTime = 0f;
    float timeIntervalToDisplayNextChar = 2f;
    float wordAnimationSpeed = 25f;

    bool endOfStep2Activated = false;
    bool endOfStep4Activated = false;
    bool endOfStep6Activated = false;
    bool endOfStep7Activated = false;

    float cameraMovementStep = 0f;
    float cameraMovementSpeed = 0.5f;

    TextMeshProUGUI firstIntroText;
    TextMeshProUGUI secondIntroText;
    TextMeshProUGUI thirdIntroText;

    Controller controller;
    Outline outline;
    GameObject earthObject;
    GameObject moonObject;

    int introCurrentStep = 1;

    float cameraSpeed = 100f;
    float cameraInitialPosition = 5f;

    private void Start()
    {
        // Show tutorial only the 1st time the 1st scene is loaded.
        if (LevelManager.playedFirstLevel)
        {
            GetComponent<GameIntro>().enabled = false;
        }
        else
        {
            // Disable buttons
            pauseButton = pauseButtonObject.GetComponent<Button>();
            earthButton = earthButtonObject.GetComponent<Button>();
            moonButton = moonButtonObject.GetComponent<Button>();
            pauseButtonColor = pauseButton.colors;
            earthButtonColor = earthButton.colors;
            moonButtonColor = moonButton.colors;
            DisableGUIButtons();
            // Items for phase 1
            earthObject = GameObject.Find("PS_Planet_Earth");
            controller = earthObject.GetComponent<Controller>();
            controller.canPlay = false;
            outline = earthObject.GetComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineHidden;
            sceneCamera.GetComponent<CameraFollowObject>().enabled = false;
            sceneCamera.orthographicSize = 200f;

            // Items for phase 2
            firstIntroText = firstIntroTextChildrenObject.GetComponent<TextMeshProUGUI>();
            firstIntroText.text = "" + firstIntroSentence[0];

            // Items for phase 3
            moonObject = GameObject.Find("PS_Satellite_Moon");
        }
    }

    private void Update()
    {
        if (introCurrentStep == 1)
        {
            // Phase 1 - Move the camera to the earth
            sceneCamera.orthographicSize -= Time.deltaTime * cameraSpeed;
            if (sceneCamera.orthographicSize < cameraInitialPosition)
            {
                introCurrentStep = 2;
                sceneCamera.orthographicSize = cameraInitialPosition;
            }
        }
        if (introCurrentStep == 2)
        {
            if (!firstIntroTextObject.activeSelf)
            {
                firstIntroTextObject.SetActive(true);
            }
            // Phase 2 - Display text for earth
            if (index == firstIntroSentence.Length)
            {
                introCurrentStep = 3;
            }
            else
            {
                if (currentTime >= timeIntervalToDisplayNextChar)
                {
                    PlaySoundFX(lettersSoundFX);
                    firstIntroText.text += "" + firstIntroSentence[index++];
                    currentTime = 0f;
                }
                else
                {
                    currentTime += Time.deltaTime * wordAnimationSpeed;
                }
            }
        }

        if (introCurrentStep == 3)
        {
            // Finalize all that are pending from step 2
            if (!endOfStep2Activated)
            {
                if (currentTime >= 10f * timeIntervalToDisplayNextChar)
                {
                    endOfStep2Activated = true;
                    firstIntroTextObject.SetActive(false);
                    PlaySoundFX(moveCameraSoundFX);
                }
                else
                {
                    currentTime += Time.deltaTime * wordAnimationSpeed;
                }
            }
            // Phase 3 - Move camera to Target
            else
            {
                Vector3 endPosition = moonObject.transform.position;
                Vector3 currentCamPosition = Vector3.Lerp(Vector3.zero,
                    endPosition, cameraMovementStep);
                currentCamPosition.z = -10f;
                sceneCamera.transform.position = currentCamPosition;
                cameraMovementStep += cameraMovementSpeed * Time.deltaTime;

                float mag = Vector3.Magnitude(endPosition - sceneCamera.transform.position +
                    new Vector3(0, 0, -10f));
                if (mag < 0.1f)
                {
                    introCurrentStep = 4;
                    currentTime = 0f;
                    index = 1;
                    secondIntroTextObject.SetActive(true);
                    secondIntroText = secondIntroTextChildrenObject.GetComponent<TextMeshProUGUI>();
                    secondIntroText.text = "" + secondIntroSentence[0];
                }
            }
        }

        if (introCurrentStep == 4)
        {
            // Phase 4 - Display text for target/moon
            sceneCamera.transform.position = moonObject.transform.position +
                new Vector3(0, 0, -10f);
            if (index == secondIntroSentence.Length)
            {
                introCurrentStep = 5;
            }
            else
            {
                if (currentTime >= timeIntervalToDisplayNextChar)
                {
                    PlaySoundFX(lettersSoundFX);
                    secondIntroText.text += "" + secondIntroSentence[index++];
                    currentTime = 0f;
                }
                else
                {
                    currentTime += Time.deltaTime * wordAnimationSpeed;
                }
            }
        }

        if (introCurrentStep == 5)
        {
            // Finalize all that are pending from step 4
            if (!endOfStep4Activated)
            {
                sceneCamera.transform.position = moonObject.transform.position +
                    new Vector3(0, 0, -10f);
                if (currentTime >= 10f * timeIntervalToDisplayNextChar)
                {
                    endOfStep4Activated = true;
                    secondIntroTextObject.SetActive(false);
                    cameraMovementStep = 0f;
                    PlaySoundFX(moveCameraSoundFX);
                }
                else
                {
                    currentTime += Time.deltaTime * wordAnimationSpeed;
                }
            }
            // Phase 5 - Move back to earth
            else
            {
                Vector3 startPosition = moonObject.transform.position;
                Vector3 currentCamPosition = Vector3.Lerp(startPosition,
                    Vector3.zero, cameraMovementStep);
                currentCamPosition.z = -10f;
                sceneCamera.transform.position = currentCamPosition;
                cameraMovementStep += cameraMovementSpeed * Time.deltaTime;

                float mag = Vector3.Magnitude(sceneCamera.transform.position +
                    new Vector3(0, 0, 10f));
                if (mag < 0.1f)
                {
                    introCurrentStep = 6;
                }
            }
        }

        if (introCurrentStep == 6)
        {
            if (!thirdIntroTextObject.activeSelf)
            {
                thirdIntroTextObject.SetActive(true);
                thirdIntroText = thirdIntroTextChildrenObject.GetComponent<TextMeshProUGUI>();
                thirdIntroText.text = "" + thirdIntroSentence[0];
                index = 1;
                currentTime = 0f;
            }
            // Phase 6 - Display game's mechanics labels, part 1
            if (index == thirdIntroSentence.Length)
            {
                introCurrentStep = 7;
            }
            else
            {
                if (currentTime >= timeIntervalToDisplayNextChar)
                {
                    PlaySoundFX(lettersSoundFX);
                    thirdIntroText.text += "" + thirdIntroSentence[index++];
                    currentTime = 0f;
                }
                else
                {
                    currentTime += Time.deltaTime * wordAnimationSpeed;
                }
            }
        }

        if (introCurrentStep == 7)
        {
            if (!endOfStep6Activated)
            {
                if (currentTime >= 10f * timeIntervalToDisplayNextChar)
                {
                    endOfStep6Activated = true;
                    thirdIntroText.text = "" + fourthIntroSentence[0];
                    index = 1;
                    currentTime = 0f;
                }
                else
                {
                    currentTime += Time.deltaTime * wordAnimationSpeed;
                }
            }
            else
            {
                // Phase 7 - Display game's mechanics labels, part 2
                if (index == fourthIntroSentence.Length)
                {
                    introCurrentStep = 8;
                }
                else
                {
                    if (currentTime >= timeIntervalToDisplayNextChar)
                    {
                        PlaySoundFX(lettersSoundFX);
                        thirdIntroText.text += "" + fourthIntroSentence[index++];
                        currentTime = 0f;
                    }
                    else
                    {
                        currentTime += Time.deltaTime * wordAnimationSpeed;
                    }
                }
            }
        }

        if (introCurrentStep == 8)
        {
            if (!endOfStep7Activated)
            {
                if (currentTime >= 10f * timeIntervalToDisplayNextChar)
                {
                    endOfStep7Activated = true;
                }
                else
                {
                    currentTime += Time.deltaTime * wordAnimationSpeed;
                }
            }
            else
            {
                // Phase 8 - Get out of tutorial!!!
                thirdIntroTextObject.SetActive(false);
                controller.canPlay = true;
                outline.OutlineMode = Outline.Mode.OutlineAll;
                sceneCamera.GetComponent<CameraFollowObject>().enabled = true;
                sceneCamera.orthographicSize = 5f;
                PlaySoundFX(moveCameraSoundFX);
                EnableGUIButtons();
                // Tutorial has been finished!!!
                LevelManager.playedFirstLevel = true;
                GetComponent<GameIntro>().enabled = false;
            }
        }
    }

    void PlaySoundFX(AudioClip audioClip)
    {
        AudioSource audioSource = GameObject.Find("SoundFX").AddComponent<AudioSource>();
        audioSource.volume = SoundManager.getVolume();
        audioSource.PlayOneShot(audioClip);
    }

    void DisableGUIButtons()
    {
        pauseButton.interactable = false;
        ColorBlock colors = pauseButtonColor;
        colors.normalColor = Color.gray;
        pauseButton.colors = colors;
        pauseButton.interactable = false;

        earthButton.interactable = false;
        colors = pauseButtonColor;
        colors.normalColor = Color.gray;
        earthButton.colors = colors;
        earthButton.interactable = false;

        moonButton.interactable = false;
        colors = pauseButtonColor;
        colors.normalColor = Color.gray;
        moonButton.colors = colors;
        moonButton.interactable = false;
    }

    void EnableGUIButtons()
    {
        pauseButton.colors = pauseButtonColor;
        earthButton.colors = earthButtonColor;
        moonButton.colors = moonButtonColor;

        pauseButton.interactable = true;
        earthButton.interactable = true;
        moonButton.interactable = true;
    }
}
