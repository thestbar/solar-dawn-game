using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public Button targetButton;
    public bool isGamePaused = false;
    public GameObject pauseBlackPane;
    public GameObject pausePaneAndTitle;
    public GameObject pauseTitle;
    public GameObject pauseRestartButton;
    public GameObject pauseRestartTitle;
    public GameObject pauseMainMenuButton;
    public GameObject pauseMainMenuTitle;
    public GameObject pauseSettingsButton;
    public GameObject pauseSettingsTitle;
    public GameObject settingsObject;
    public int transitionSpeed = 5;
    public GameObject soundFXObject;
    public AudioClip buttonSoundFX;
    private AudioSource[] audioSources;
    private float beforePauseTimeScale;
    private Controller controller;
    private bool pauseCoroutineIsActive = false;

    void Start()
    {
        Button btn = targetButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        controller = GameObject.Find("PS_Planet_Earth").GetComponent<Controller>();
        pauseBlackPane.SetActive(false);
        Color objectColor = pauseBlackPane.GetComponent<Image>().color;
        objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, 0f);
        pauseBlackPane.GetComponent<Image>().color = objectColor;
    }

    void TaskOnClick()
    {
        if (pauseCoroutineIsActive) return;
        GameObject soundFXObject = GameObject.Find("SoundFX");
        AudioSource soundFXAudioSource = soundFXObject.AddComponent<AudioSource>();
        soundFXAudioSource.volume = SoundManager.getVolume();
        soundFXAudioSource.PlayOneShot(buttonSoundFX);
        audioSources = soundFXObject.GetComponents<AudioSource>();
        if (!isGamePaused)
        {
            foreach (AudioSource audio in audioSources)
            {
                if (audio == soundFXAudioSource) continue;
                audio.Pause();
            }
            beforePauseTimeScale = Time.timeScale;
            controller.canPlay = false;
            StartCoroutine(ShowMenu());
            isGamePaused = true;
        } else
        {
            foreach (AudioSource audio in audioSources)
            {
                audio.UnPause();
            }
            controller.canPlay = true;
            StartCoroutine(HideMenu());
            isGamePaused = false;
        }
    }

    IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, int fadeSpeed = 5)
    {
        Color objectColor = pauseBlackPane.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (pauseBlackPane.GetComponent<Image>().color.a < 0.9)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                pauseBlackPane.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
        else
        {
            while(pauseBlackPane.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                pauseBlackPane.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
    }

    // First makes the smooth transition for the alpha of the black pane
    // And then stops the time (sets timeScale = 0)
    // If the timeScale is zero then the coroutines are not executed
    IEnumerator ShowMenu()
    {
        pauseCoroutineIsActive = true;
        pauseBlackPane.SetActive(true);
        pausePaneAndTitle.SetActive(true);
        pauseTitle.SetActive(true);
        pauseRestartButton.SetActive(true);
        pauseRestartTitle.SetActive(true);
        pauseMainMenuButton.SetActive(true);
        pauseMainMenuTitle.SetActive(true);
        pauseSettingsButton.SetActive(true);
        pauseSettingsTitle.SetActive(true);
        yield return StartCoroutine(FadeBlackOutSquare(true, transitionSpeed));
        Time.timeScale = 0f;
        pauseCoroutineIsActive = false;
    }

    IEnumerator HideMenu()
    {
        pauseCoroutineIsActive = true;
        Time.timeScale = beforePauseTimeScale;
        yield return StartCoroutine(FadeBlackOutSquare(false, transitionSpeed));
        pauseBlackPane.SetActive(false);
        pausePaneAndTitle.SetActive(false);
        pauseTitle.SetActive(false);
        pauseRestartButton.SetActive(false);
        pauseRestartTitle.SetActive(false);
        pauseMainMenuButton.SetActive(false);
        pauseMainMenuTitle.SetActive(false);
        pauseSettingsButton.SetActive(false);
        pauseSettingsTitle.SetActive(false);
        pauseCoroutineIsActive = false;
        if (settingsObject != null) settingsObject.SetActive(false);
    }
}
