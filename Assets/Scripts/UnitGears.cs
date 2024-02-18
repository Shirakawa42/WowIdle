public class UnitGears
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
}
