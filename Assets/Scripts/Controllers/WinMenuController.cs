using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LevelManager;

public class WinMenuController : MonoBehaviour
{
    public int levelId;
    public AudioClip loseSoundFX;
    public AudioClip winSoundFX;
    private GameObject[] objects;
    private GameObject winBlackPane;
    private GameObject probe;
    private SatelliteController satelliteController;
    private bool winScreenCoroutineIsActive = false;
    private bool calculateFinalScore = false;
    private bool playWinScreenSoundFX = false;
    private Image star1;
    private Image star1Grey;
    private Image star2;
    private Image star2Grey;
    private Image star3;
    private Image star3Grey;
    private GameObject dataScore;

    private void Start()
    {
        objects = GameObject.FindGameObjectsWithTag("WinScreen");
        star1 = GameObject.Find("Canvas").transform.GetChild(28).gameObject.GetComponent<Image>();
        star1Grey = GameObject.Find("Canvas").transform.GetChild(27).gameObject.GetComponent<Image>();
        star2 = GameObject.Find("Canvas").transform.GetChild(30).gameObject.GetComponent<Image>();
        star2Grey = GameObject.Find("Canvas").transform.GetChild(29).gameObject.GetComponent<Image>();
        star3 = GameObject.Find("Canvas").transform.GetChild(32).gameObject.GetComponent<Image>();
        star3Grey = GameObject.Find("Canvas").transform.GetChild(31).gameObject.GetComponent<Image>();
        dataScore = GameObject.Find("Canvas").transform.GetChild(33).gameObject.transform.GetChild(0).gameObject;
        star1.enabled = false;
        star2.enabled = false;
        star3.enabled = false;
        foreach (GameObject o in objects)
        {
            o.SetActive(false);
        }
    }

    void Update()
    {
        if (winScreenCoroutineIsActive) return;
        probe = GameObject.FindGameObjectWithTag("Probe");
        if(probe != null)
        {
            satelliteController = probe.GetComponent<SatelliteController>();
            if (satelliteController.IsWin())
            {
                StartCoroutine(ShowWinScreen(satelliteController.score));
                if(!calculateFinalScore) StartCoroutine(ShowFinalScore());   
            }
        }
    }

    IEnumerator ShowFinalScore(int iterations = 100, float secondsToWaitBetweenIterations = 0.01f)
    {
        calculateFinalScore = true;
        float finalNum = satelliteController.score;
        float showNum = 0.0f;
        for(int i = 0; i < iterations; i++)
        {
            WriteScoreToScreen(showNum);
            WriteStarsValues(showNum);
            showNum += finalNum / iterations;
            yield return new WaitForSeconds(secondsToWaitBetweenIterations);
        }
        WriteScoreToScreen(finalNum);
        WriteStarsValues(finalNum);
    }

    private void WriteStarsValues(float score)
    {
        if(score < 1000f)
        {
            star1.enabled = true;
            star1.fillAmount = score / 1000f;
        } else if (score < 2500f)
        {
            star1.enabled = true;
            star2.enabled = true;
            star1.fillAmount = 1f;
            star1Grey.enabled = false;
            star2.fillAmount = (score - 1000f) / 1500f;
        } else if (score < 5000f)
        {
            star1.enabled = true;
            star2.enabled = true;
            star3.enabled = true;
            star1.fillAmount = 1f;
            star2.fillAmount = 1f;
            star1Grey.enabled = false;
            star2Grey.enabled = false;
            star3.fillAmount = (score - 2500f) / 2500f;
        } else
        {
            star1.enabled = true;
            star2.enabled = true;
            star3.enabled = true;
            star1.fillAmount = 1f;
            star2.fillAmount = 1f;
            star3.fillAmount = 1f;
            star1Grey.enabled = false;
            star2Grey.enabled = false;
            star3Grey.enabled = false;
        }
    }

    private void WriteScoreToScreen(float score)
    {
        string text = "";
        if (score < 1000)
        {
            text = score.ToString("0.00") + " KB";
        }
        else
        {
            text = (score / 1000f).ToString("0.00") + " MB";
        }
        dataScore.GetComponent<TextMeshProUGUI>().SetText(text);
    }

    IEnumerator ShowWinScreen(float score)
    {
        PlayWinScreenSFX(score);
        playWinScreenSoundFX = true;
        winScreenCoroutineIsActive = true;
        if (score >= 1000f)
        {
            LevelManager.LevelInfo[] levelInfos = LevelManager.levelInfos;
            LevelManager.LevelInfo myLevelInfo = levelInfos[levelId];
            myLevelInfo.score = Mathf.Max(score, myLevelInfo.score);
            myLevelInfo.levelPassed = true;
            int nextLevelId = levelId + 1;
            if (nextLevelId < 9)
            {
                LevelManager.LevelInfo nextLevelInfo = levelInfos[nextLevelId];
                nextLevelInfo.isPlayable = true;
                Debug.Log("Win Menu Controller - Is next level playable? " + nextLevelInfo.isPlayable + " - Next Level ID: " +
                nextLevelInfo.levelId);
            }
        }

        foreach (GameObject o in objects)
        {
            if (o.name == "ActionText_MissionClear" && score >= 1000f)
            {
                o.SetActive(true);
            }
            else if (o.name == "ActionText_MissionClear" && score < 1000f)
            {
                o.SetActive(false);
            }
            else if (o.name == "ActionText_GameOver" && score < 1000f)
            {
                o.SetActive(true);
            }
            else if (o.name == "ActionText_GameOver" && score >= 1000f)
            {
                o.SetActive(false);
            }
            else
            {
                o.SetActive(true);
            }
        }
        yield return null;
        winScreenCoroutineIsActive = false;
    }

    IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, int fadeSpeed = 5)
    {
        Color objectColor = winBlackPane.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (winBlackPane.GetComponent<Image>().color.a < 0.9f)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                winBlackPane.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
        else
        {
            while (winBlackPane.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                winBlackPane.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
    }

    void PlayWinScreenSFX(float score)
    {
        if (playWinScreenSoundFX) return;
        GameObject soundFXObj = GameObject.Find("SoundFX");
        AudioSource soundFXAudioSource = soundFXObj.AddComponent<AudioSource>();
        soundFXAudioSource.volume = SoundManager.getVolume();
        if (score >= 1000f) soundFXAudioSource.PlayOneShot(winSoundFX);
        else soundFXAudioSource.PlayOneShot(loseSoundFX);
    }
}
