using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StatsPanelManager : MonoBehaviour
{
    public GameObject statsNames;
    public GameObject statsValues;

    private List<TMP_Text>  names;
    private List<TMP_Text> values;

    void Awake()
    {
        Globals.statsPanelManager = this;
        names = new List<TMP_Text>();
        values = new List<TMP_Text>();
        foreach (Transform child in statsNames.transform)
        {
            names.Add(child.GetComponent<TMP_Text>());
        }
        foreach (Transform child in statsValues.transform)
        {
            values.Add(child.GetComponent<TMP_Text>());
        }
    }

    public void UpdateStats(Unit unit)
    {
        if (unit == null)
            return;

        Stats stats = unit.stats;
        UnitStats unitStats = unit.unitStats;

        (string, int)[] allUnitStats = unitStats.GetAllStats();
        (string, int)[] allStats = stats.GetAllStats();

        (string, int)[] concatenedStats = allUnitStats.Concat(allStats).ToArray();

        for (int i = 0; i < names.Count; i++)
        {
            if (i >= concatenedStats.Length)
            {
                names[i].gameObject.SetActive(false);
                values[i].gameObject.SetActive(false);
                continue;
            }
            names[i].gameObject.SetActive(true);
            values[i].gameObject.SetActive(true);
            names[i].text = concatenedStats[i].Item1;
            values[i].text = concatenedStats[i].Item2.ToString();
            Color color = unitStats.GetStatColor(concatenedStats[i].Item1);
            names[i].color = color;
            values[i].color = color;
        }
    }
}