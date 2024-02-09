using UnityEngine;

public abstract class Slotable
{
    public abstract SlotType SlotType { get; set; }
    public abstract Sprite Icon { get; set; }
    public abstract Color Color { get; set; }

    public abstract void OnEquip(Unit unit, Slot slot);
    public abstract void OnUnequip(Slot slot);
    public abstract void OnPointerEnter(Vector3 slotPosition);
    public abstract void OnPointerExit();
}
