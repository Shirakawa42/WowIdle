using System.Collections.Generic;
using UnityEngine;

public class HeroesInventoryManager : MonoBehaviour
{
    public GameObject heroesPanel;
    public GameObject slotPrefab;
    public int inventoryLinesAmount = 10;
    public int spacing = -105;

    private List<Slot> heroeslots;

    void Awake()
    {
        Globals.heroesInventoryManager = this;
        heroeslots = new List<Slot>();
        for (int i = 1; i < inventoryLinesAmount; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                GameObject slot = Instantiate(slotPrefab, heroesPanel.transform);
                slot.GetComponent<Slot>().slotType = SlotType.Hero;
                slot.GetComponent<Slot>().equippedSlot = false;
                slot.GetComponent<RectTransform>().anchorMax = heroesPanel.transform.GetChild(j).GetComponent<RectTransform>().anchorMax;
                slot.GetComponent<RectTransform>().anchorMin = heroesPanel.transform.GetChild(j).GetComponent<RectTransform>().anchorMin;
                slot.GetComponent<RectTransform>().anchoredPosition = heroesPanel.transform.GetChild(j).GetComponent<RectTransform>().anchoredPosition + new Vector2(0, spacing * i);
            }

        }
        foreach (Transform child in heroesPanel.transform)
        {
            heroeslots.Add(child.GetComponent<Slot>());
        }
    }

    public void AddHero(Unit unit)
    {
        foreach (Slot slot in heroeslots)
        {
            if (slot.IsEmpty())
            {
                slot.SetSlotable(unit);
                return;
            }
        }
    }
}
