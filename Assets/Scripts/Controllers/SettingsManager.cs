using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject sliderObject;
    public GameObject volumeTextObject;
    public GameObject audioSourceObject;
    public GameObject volumeIconObject;
    public GameObject maxVolumeTextObject;
    public GameObject minVolObj;
    public GameObject maxVolObj;
    public GameObject muteOn;
    public GameObject muteOff;
    private AudioSource audioSource;
    private Slider volumeSlider;
    private TextMeshProUGUI volumeText;
    private TextMeshProUGUI maxVolumeText;
    private const float MIN_POS_VOL = -217.5f;
    private const float MAX_POS_VOL = 217.5f;
    private const float FULL_POS = 217.5f * 2f;

    private void Start()
    {
        volumeText = volumeTextObject.GetComponent<TextMeshProUGUI>();
        maxVolumeText = maxVolumeTextObject.GetComponent<TextMeshProUGUI>();
        volumeSlider = sliderObject.GetComponent<Slider>();
        audioSource = audioSourceObject.GetComponent<AudioSource>();
        volumeSlider.value = SoundManager.getVolume();
        float currentPos = FULL_POS * volumeSlider.value - MAX_POS_VOL;
        volumeIconObject.transform.localPosition = new Vector3(
            currentPos,
            volumeIconObject.transform.localPosition.y, 0);
    }

    private void Update()
    {
        float newVolume = volumeSlider.value;
        volumeText.text = (newVolume * 100f).ToString("###");
        SoundManager.setVolume(newVolume);
        audioSource.volume = newVolume;

        float currentPos = FULL_POS * newVolume - MAX_POS_VOL;

        volumeIconObject.transform.localPosition = new Vector3(
            currentPos,
            volumeIconObject.transform.localPosition.y, 0);

        if(newVolume < 0.01f)
        {
            SoundManager.setVolume(0f);
            audioSource.volume = 0f;
            volumeIconObject.transform.localPosition = new Vector3(
            MIN_POS_VOL,
            volumeIconObject.transform.localPosition.y, 0);
            volumeText.text = "0";
            minVolObj.SetActive(false);
            muteOn.SetActive(true);
            muteOff.SetActive(false);
        }
        else
        {
            minVolObj.SetActive(true);
            muteOn.SetActive(false);
            muteOff.SetActive(true);
        }

        if(newVolume > 0.90f)
        {
            maxVolumeText.text = "";
        }
        else
        {
            maxVolumeText.text = "100";
        }

        if(newVolume > 0.99f)
        {
            maxVolObj.SetActive(false);
        }
        else
        {
            maxVolObj.SetActive(true);
        }
    }
}
