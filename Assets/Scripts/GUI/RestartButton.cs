using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    public Button restartButton;
    public string sceneKeyValue = "0";
    public AudioClip buttonSoundFX;

    void Start()
    {
        Button btn = restartButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        GameObject soundFXObject = GameObject.Find("SoundFX");
        AudioSource soundFXAudioSource = soundFXObject.AddComponent<AudioSource>();
        soundFXAudioSource.volume = SoundManager.getVolume();
        soundFXAudioSource.PlayOneShot(buttonSoundFX);
        Enum.TryParse(sceneKeyValue, out Loader.Scene scene);
        Loader.Load(scene);
        Time.timeScale = 1f;
    }
}
