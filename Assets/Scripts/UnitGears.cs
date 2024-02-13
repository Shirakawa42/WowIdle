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
        return (Weapon)gearSlots[14];
    }

    public Weapon GetOffHandWeapon()
    {
        return (Weapon)gearSlots[15];
    }

    public Equipment GetEquipment(int gearSlotId)
    {
        return gearSlots[gearSlotId];
    }
}
