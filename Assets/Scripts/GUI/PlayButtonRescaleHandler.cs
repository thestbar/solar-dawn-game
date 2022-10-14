using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonRescaleHandler : MonoBehaviour
{
    public GameObject earthObject;
    public GameObject buttonObject;

    private void Update()
    {
        float aspectRatio = Screen.width * 1.0f / Screen.height;
        float scale = calculateScaleForAspectRatio(aspectRatio);
        earthObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    float calculateScaleForAspectRatio(float aspectRatio)
    {
        if (aspectRatio >= 1.7f)
            return 1f;
        if (aspectRatio < 0.5f)
            return 0.3f;
        float slope = 0.56f;
        float intercept = 0.02f;
        float result = slope * aspectRatio + intercept;
        return result;
    }
}
