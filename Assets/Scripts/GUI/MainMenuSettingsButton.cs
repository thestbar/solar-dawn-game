using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettingsButton : MonoBehaviour
{
    public Button settingsButton;

    public GameObject missionsButton;
    public GameObject startButton;
    public GameObject title;
    public GameObject settingsPane;
    public GameObject settingsButtonGameObject;
    public AudioClip buttonSoundFX;

    void Start()
    {
        Button btn = settingsButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        GameObject soundFXObject = GameObject.Find("SoundFX");
        AudioSource soundFXAudioSource = soundFXObject.AddComponent<AudioSource>();
        soundFXAudioSource.volume = SoundManager.getVolume();
        soundFXAudioSource.PlayOneShot(buttonSoundFX);
        missionsButton.SetActive(false);
        startButton.SetActive(false);
        title.SetActive(false);
        settingsButtonGameObject.SetActive(false);
        settingsPane.SetActive(true);
    }
}
