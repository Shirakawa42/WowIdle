using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image filledImage;
    public TMP_Text text;
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateValues(int max, int current, string color)
    {
        slider.maxValue = max;
        slider.value = current;
        slider.fillRect.GetComponent<Image>().color = ColorUtils.GetColorFromHex(color);
        text.text = current + " / " + max;
    }
}
