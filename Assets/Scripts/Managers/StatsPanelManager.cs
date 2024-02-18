using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StatsPanelManager : MonoBehaviour
{
    public TextMeshProUGUI statsNames;
    public TextMeshProUGUI statsValues;

    void Awake()
    {
        Globals.statsPanelManager = this;
        statsNames.SetText("");
        statsValues.SetText("");
    }

    public void UpdateStats(Unit unit)
    {
        if (unit == null)
            return;

        Stat[] stats = unit.stats.GetUsedStats();

        string text = "<align=left>";
        foreach (Stat stat in stats)
            if (stat.type != ValueType.Invisible)
                text += $"<color={ColorUtils.GetColorFromValueType(stat.type)}>{stat.name}</color>\n";
        text += "</align>";
        statsNames.SetText(text);

        text = "<align=right>";
        foreach (Stat stat in stats)
            if (stat.type != ValueType.Invisible)
                text += $"<color={ColorUtils.GetColorFromValueType(stat.type)}>{stat.value}{(stat.type == ValueType.SecondaryStatPercent ? "%" : "")}</color>\n";
        text += "</align>";
        statsValues.SetText(text);
    }
}