using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MuteOffButtonHandler : MonoBehaviour, IPointerClickHandler
{
    public AudioSource audioSource;
    public GameObject muteOn;
    public GameObject muteOff;
    public GameObject volumeIconObject;
    public GameObject minVolObj;
    public GameObject sliderObj;
    public GameObject volumeTextObject;
    public AudioClip buttonSoundFX;
    private const float MIN_POS_VOL = -217.5f;
    private TextMeshProUGUI volumeText;
    private Slider slider;

    void Awake()
    {
        volumeText = volumeTextObject.GetComponent<TextMeshProUGUI>();
        slider = sliderObj.GetComponent<Slider>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject soundFXObject = GameObject.Find("SoundFX");
        AudioSource soundFXAudioSource = soundFXObject.AddComponent<AudioSource>();
        soundFXAudioSource.volume = SoundManager.getVolume();
        soundFXAudioSource.PlayOneShot(buttonSoundFX);
        slider.value = 0f;
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

}
