using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingProgressBar : MonoBehaviour
{
    private Slider slider;
    private TMP_Text text;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        text = GameObject.Find("Percentage_Text").GetComponent<TMP_Text>();
    }

    private void Update()
    {
        float num = Loader.GetLoadingProgress();
        slider.value = num;
        text.text = Mathf.Round((num * 100)).ToString() + "%";
    }
}
