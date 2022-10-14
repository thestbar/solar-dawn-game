using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionLevelHandler : MonoBehaviour
{
    public int levelId;
    private bool isPlayable;
    private float score;
    private bool levelPassed;

    private void Start()
    {
        LevelManager.LevelInfo[] levelInfos = LevelManager.levelInfos;
        LevelManager.LevelInfo myLevelInfo = levelInfos[levelId - 1];

        isPlayable = myLevelInfo.isPlayable;
        score = myLevelInfo.score;
        levelPassed = myLevelInfo.levelPassed;

        if(!isPlayable)
        {
            Debug.Log("Hello from inside");
            var colors = GetComponent<Button>().colors;
            colors.normalColor = Color.gray;
            GetComponent<Button>().colors = colors;
            GetComponent<Button>().interactable = false;
        } 
    }
}
