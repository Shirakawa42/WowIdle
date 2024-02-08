using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct TooltipValue {
    public string name;
    public string value;
    public Color color;

    public TooltipValue(string name, string value, Color color)
    {
        this.name = name;
        this.value = value;
        this.color = color;
    }
}

public class ItemTooltipManager : MonoBehaviour
{
    public GameObject tooltip;
    public Image border;
    public GameObject p_names;
    public GameObject p_values;
    public TMP_Text title;
    private List<TMP_Text> names;
    private List<TMP_Text> values;
    private int maxLines;

    void Awake()
    {
        Globals.itemTooltipManager = this;
        tooltip.SetActive(false);
        names = new List<TMP_Text>();
        values = new List<TMP_Text>();
        foreach (Transform child in p_names.transform)
        {
            names.Add(child.GetComponent<TMP_Text>());
        }
        foreach (Transform child in p_values.transform)
        {
            values.Add(child.GetComponent<TMP_Text>());
        }
        maxLines = names.Count;
    }

    public void ShowTooltip(List<TooltipValue> tooltipValues, Color borderColor, Vector3 position, string title, Color titleColor)
    {
        this.title.text = title;
        this.title.color = titleColor;
        for (int i = 0; i < maxLines; i++)
        {
            if (i >= tooltipValues.Count)
            {
                names[i].gameObject.SetActive(false);
                values[i].gameObject.SetActive(false);
                continue;
            }
            names[i].gameObject.SetActive(true);
            values[i].gameObject.SetActive(true);
            names[i].text = tooltipValues[i].name;
            names[i].color = tooltipValues[i].color;
            values[i].text = tooltipValues[i].value;
            values[i].color = tooltipValues[i].color;
        }
        border.color = borderColor;
        tooltip.transform.position = position;
        tooltip.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
    
}
