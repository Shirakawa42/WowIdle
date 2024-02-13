using TMPro;
using UnityEngine;

public class GearSlotsManager : MonoBehaviour
{
    public UnitGears currentGears;
    public Slot[] gearSlots;
    public TMP_Text heroName;

    void Awake()
    {
        Globals.gearSlotsManager = this;
        EnableGearSlots(false);
        heroName.text = "";
    }

    public void SetGearSlots(UnitGears gears, string name)
    {
        currentGears = gears;
        EnableGearSlots(currentGears != null);
        for (int i = 0; i < gearSlots.Length; i++)
        {
            gearSlots[i].SetSlotable(gears.GetEquipment(i), true, true);
        }
        heroName.text = name;
    }

    private void EnableGearSlots(bool enable)
    {
        foreach (Slot slot in gearSlots)
        {
            slot.gameObject.SetActive(enable);
        }
    }
}
