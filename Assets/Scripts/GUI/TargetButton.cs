using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetButton : MonoBehaviour
{
    public Camera cam;
    public GameObject target;
    public Button targetButton;
    public AudioClip buttonSoundFX;
    private CameraFollowObject cameraFollowObject;
    private bool canPlay;

    void Start()
    {
        Button btn = targetButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        cameraFollowObject = cam.GetComponent<CameraFollowObject>();
    }

    void TaskOnClick()
    {
        GameObject soundFXObject = GameObject.Find("SoundFX");
        AudioSource soundFXAudioSource = soundFXObject.AddComponent<AudioSource>();
        soundFXAudioSource.volume = SoundManager.getVolume();
        soundFXAudioSource.PlayOneShot(buttonSoundFX);
        canPlay = GameObject.Find("PS_Planet_Earth").GetComponent<Controller>().canPlay;
        if (GameObject.FindGameObjectWithTag("Rocket") == null && GameObject.FindGameObjectWithTag("Probe") == null && canPlay)
        {
            cameraFollowObject.NewObjectToFollow(target,
                new Vector3(cam.transform.position.x, cam.transform.position.y, -10f));
        }
    }
}
