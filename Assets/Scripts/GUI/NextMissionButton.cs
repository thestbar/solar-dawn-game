using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextMissionButton : MonoBehaviour
{
    public Button nextMissionButton;
    public string nextSceneKeyValue = "0";
    public int nextLevelId;
    public AudioClip buttonSoundFX;

    void Start()
    {
        Button btn = nextMissionButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        if (nextLevelId == -1)
        {
            var colors = GetComponent<Button>().colors;
            colors.normalColor = Color.gray;
            GetComponent<Button>().colors = colors;
            GetComponent<Button>().interactable = false;
        }
    }

    void TaskOnClick()
    {
        GameObject soundFXObject = GameObject.Find("SoundFX");
        AudioSource soundFXAudioSource = soundFXObject.AddComponent<AudioSource>();
        soundFXAudioSource.volume = SoundManager.getVolume();
        soundFXAudioSource.PlayOneShot(buttonSoundFX);
        Enum.TryParse(nextSceneKeyValue, out Loader.Scene scene);
        Loader.Load(scene);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (nextLevelId == -1) return;
        LevelManager.LevelInfo[] levelInfos = LevelManager.levelInfos;
        LevelManager.LevelInfo nextLevelInfo = levelInfos[nextLevelId - 1];
            
        if (!nextLevelInfo.isPlayable)
        {
            var colors = GetComponent<Button>().colors;
            colors.normalColor = Color.gray;
            GetComponent<Button>().colors = colors;
            GetComponent<Button>().interactable = false;
        }
        else
        {
            var colors = GetComponent<Button>().colors;
            colors.normalColor = Color.white;
            GetComponent<Button>().colors = colors;
            GetComponent<Button>().interactable = true;
        }
    }
}
