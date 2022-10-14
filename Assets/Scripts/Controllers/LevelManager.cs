using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelManager
{
    public class LevelInfo
    {
        public int levelId;
        public bool isPlayable;
        public float score;
        public bool levelPassed;

        public LevelInfo(int levelId)
        {
            this.levelId = levelId;
            this.levelPassed = false;
            if (levelId == 1) this.isPlayable = true;
            else this.isPlayable = false;
            this.score = 0;
        }
    }

    public static LevelInfo[] levelInfos =
    {
        new LevelInfo(1),
        new LevelInfo(2),
        new LevelInfo(3),
        new LevelInfo(4),
        new LevelInfo(5),
        new LevelInfo(6),
        new LevelInfo(7),
        new LevelInfo(8),
        new LevelInfo(9)
    };

    public static bool playedFirstLevel = false;
}
