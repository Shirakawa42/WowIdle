using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int inventoryColumnAmount = 10;

    private List<Slot> inventoryslots;

    void Awake()
    {
        Globals.inventoryManager = this;
        inventoryslots = new List<Slot>();
        for (int i = 1; i < inventoryColumnAmount; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject slot = Instantiate(slotPrefab, inventoryPanel.transform);
                slot.GetComponent<Slot>().slotType = SlotType.AnyGear;
                slot.GetComponent<Slot>().equippedSlot = false;
                slot.GetComponent<RectTransform>().anchorMax = inventoryPanel.transform.GetChild(j).GetComponent<RectTransform>().anchorMax;
                slot.GetComponent<RectTransform>().anchorMin = inventoryPanel.transform.GetChild(j).GetComponent<RectTransform>().anchorMin;
                slot.GetComponent<RectTransform>().anchoredPosition = inventoryPanel.transform.GetChild(j).GetComponent<RectTransform>().anchoredPosition + new Vector2(85 * i, 0);
            }
        }
        foreach (Transform child in inventoryPanel.transform)
        {
            inventoryslots.Add(child.GetComponent<Slot>());
        }
    }

    public void AddItem(Equipment equipment)
    {
        if (equipment == null) return;
        foreach (Slot slot in inventoryslots)
        {
            if (slot.IsEmpty())
            {
                slot.SetSlotable(equipment);
                return;
            }
        }
    }
}
