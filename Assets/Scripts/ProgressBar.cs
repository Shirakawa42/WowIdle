using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image filledImage;

    private Color color = Color.red;
    private int max = 100;
    private int current = 100;
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateValues(int max, int current, Color color)
    {
        this.max = max;
        this.current = current;
        slider.maxValue = max;
        slider.value = current;
        slider.fillRect.GetComponent<Image>().color = color;
    }
}
