using System;

public class UnitGears : ICloneable
{
    private readonly Equipment[] gearSlots = new Equipment[16];

    public void Equip(int gearSlotId, Equipment equipment)
    {
        gearSlots[gearSlotId] = equipment;
    }

    public void Unequip(int gearSlotId)
    {
        gearSlots[gearSlotId] = null;
    }

    public Weapon GetMainHandWeapon()
    {
        return (Weapon)gearSlots[(int)GearSlotsIds.MainHand];
    }

    public Weapon GetOffHandWeapon()
    {
        return (Weapon)gearSlots[(int)GearSlotsIds.OffHand];
    }

    public Equipment GetEquipment(int gearSlotId)
    {
        return gearSlots[gearSlotId];
    }

    public object Clone()
    {
        UnitGears unitGears = new();
        for (int i = 0; i < gearSlots.Length; i++)
        {
            if (gearSlots[i] != null)
                unitGears.gearSlots[i] = (Equipment)gearSlots[i].Clone();
        }
        return unitGears;
    }
}
