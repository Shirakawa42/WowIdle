using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public struct TooltipValue {
    public string name;
    public string value;
    public ValueType type;
    public Rarities rarity;

    public TooltipValue(string name, string value, ValueType type, Rarities rarity = Rarities.Common)
    {
        this.name = name;
        this.value = value;
        this.type = type;
        this.rarity = rarity;
    }
}

public class ItemTooltipManager : MonoBehaviour
{
    public GameObject tooltip;
    public RectTransform background;
    public RectTransform border;
    public TextMeshProUGUI text;

    private Image borderImage;

    void Awake()
    {
        Globals.itemTooltipManager = this;
        borderImage = border.GetComponent<Image>();
        HideTooltip();
    }

    void UpdateSize()
    {
        text.ForceMeshUpdate(true);
        Vector2 textSize = text.GetRenderedValues(false);
        background.sizeDelta = textSize + new Vector2(20, 20);
        border.sizeDelta = textSize + new Vector2(25, 25);
        text.rectTransform.sizeDelta = textSize;
    }

    public void ShowTooltip(List<TooltipValue> tooltipValues, string borderColor, Vector3 position)
    {
        text.SetText("");
        foreach (TooltipValue tooltipValue in tooltipValues)
        {
            if (tooltipValue.type == ValueType.MainStat || tooltipValue.type == ValueType.SecondaryStat || tooltipValue.type == ValueType.Stamina)
                text.SetText(text.text + "<align=left>" + "<color=" + ColorUtils.GetTooltipColorFromValueType(tooltipValue.type) + ">+" + tooltipValue.value + " " + tooltipValue.name + "</color></align>\n");
            else if (tooltipValue.type == ValueType.Name)
                text.SetText(text.text + "<align=left>" + "<color=" + ColorUtils.GetTooltipColorFromValueType(tooltipValue.type, tooltipValue.rarity) + ">" + tooltipValue.name + "</color></align>\n");
            else if (tooltipValue.type == ValueType.Description)
                text.SetText(text.text + "<align=left>" + "<color=" + ColorUtils.GetTooltipColorFromValueType(tooltipValue.type) + ">" + tooltipValue.name + "</color></align>\n");
            else if (tooltipValue.type == ValueType.Armor)
                text.SetText(text.text + "<align=left>" + "<color=" + ColorUtils.GetTooltipColorFromValueType(tooltipValue.type) + ">" + tooltipValue.name + ": " + tooltipValue.value + "</color></align>\n");
            else if (tooltipValue.type == ValueType.EquipmentType)
                text.SetText(text.text + "<align=left>" + "<color=" + ColorUtils.GetTooltipColorFromValueType(tooltipValue.type) + ">" + tooltipValue.name + "</color></align>\n");
            else if (tooltipValue.type == ValueType.SecondaryStatPercent)
                text.SetText(text.text + "<align=left>" + "<color=" + ColorUtils.GetTooltipColorFromValueType(tooltipValue.type) + ">" + tooltipValue.name + ": +" + tooltipValue.value + "%</color></align>\n");
            else
                text.SetText(text.text + "<align=left>" + "<color=" + ColorUtils.GetTooltipColorFromValueType(tooltipValue.type) + ">UNSET:" + tooltipValue.name + ": " + tooltipValue.value + "</color></align>\n");
        }
        borderImage.color = ColorUtils.GetColorFromHex(borderColor);
        UpdateSize();
        UpdateSize();
        tooltip.transform.position = position + new Vector3(10, -10, 0);
    }

    public void HideTooltip()
    {
        tooltip.transform.position = new Vector3(-1000, -1000, 0);
    }

}
