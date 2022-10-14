using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MuteOnButtonHandler : MonoBehaviour, IPointerClickHandler
{
    public AudioSource audioSource;
    public GameObject muteOn;
    public GameObject muteOff;
    public GameObject volumeIconObject;
    public GameObject minVolObj;
    public GameObject sliderObj;
    public GameObject volumeTextObject;
    public AudioClip buttonSoundFX;
    private TextMeshProUGUI volumeText;
    private Slider slider;

    void Awake()
    {
        volumeText = volumeTextObject.GetComponent<TextMeshProUGUI>();
        slider = sliderObj.GetComponent<Slider>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        slider.value = 0.5f;
        SoundManager.setVolume(0.5f);
        audioSource.volume = 0.5f;
        volumeIconObject.transform.localPosition = new Vector3(
        0, volumeIconObject.transform.localPosition.y, 0);
        volumeText.text = "50";
        GameObject soundFXObject = GameObject.Find("SoundFX");
        AudioSource soundFXAudioSource = soundFXObject.AddComponent<AudioSource>();
        soundFXAudioSource.volume = SoundManager.getVolume();
        soundFXAudioSource.PlayOneShot(buttonSoundFX);
        minVolObj.SetActive(true);
        muteOn.SetActive(false);
        muteOff.SetActive(true);
    }

}
