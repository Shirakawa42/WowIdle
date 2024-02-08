using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    public int columnAmount = 10;
    public GameObject slotPrefab;
    private List<Slot> slots;

    void Awake()
    {
        Globals.inventoryManager = this;
        slots = new List<Slot>();
        for (int i = 1; i < columnAmount; i++)
        {
            for (int j = 0; j < 3; j++) {
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
            slots.Add(child.GetComponent<Slot>());
        }
    }

    public void AddItem(Equipment equipment)
    {
        foreach (Slot slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.SetSlotable(equipment);
                return;
            }
        }
    }
}
