using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsReturnBackButton : MonoBehaviour
{
    public Button returnBackButton;
    public GameObject settingsObject;
    public bool isAtMainMenu;
    public AudioClip buttonSoundFX;

    // Only used for Main Menu
    public GameObject missionsButton;
    public GameObject startButton;
    public GameObject title;
    public GameObject settingsButton;
    public GameObject settingsPane;

    void Start()
    {
        Button btn = returnBackButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        GameObject soundFXObject = GameObject.Find("SoundFX");
        AudioSource soundFXAudioSource = soundFXObject.AddComponent<AudioSource>();
        soundFXAudioSource.volume = SoundManager.getVolume();
        soundFXAudioSource.PlayOneShot(buttonSoundFX);
        if (settingsObject != null) settingsObject.SetActive(false);
        if(isAtMainMenu)
        {
            missionsButton.SetActive(true);
            startButton.SetActive(true);
            title.SetActive(true);
            settingsButton.SetActive(true);
            settingsPane.SetActive(false);
        }
    }
}
