using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    private static float volume = 0.5f;
    private static float beforeMuteVolume = 0.5f;

    public static float getVolume()
    {
        return volume;
    }

    public static void setVolume(float newVolume)
    {
        volume = newVolume;
    }
}
