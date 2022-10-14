using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuMissionsButton : MonoBehaviour
{
    public Button missionsButton;
    public string targetLevel;
    public AudioClip buttonSoundFX;

    void Start()
    {
        Button btn = missionsButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        GameObject soundFXObject = GameObject.Find("SoundFX");
        AudioSource soundFXAudioSource = soundFXObject.AddComponent<AudioSource>();
        soundFXAudioSource.volume = SoundManager.getVolume();
        soundFXAudioSource.PlayOneShot(buttonSoundFX);
        Enum.TryParse(targetLevel, out Loader.Scene scene);
        Loader.Load(scene);
        Time.timeScale = 1f;
    }
}
