using UnityEngine;

public abstract class Slotable
{
    public abstract SlotType SlotType { get; set; }
    public abstract Sprite Icon { get; set; }
    public abstract Color Color { get; set; }

    public abstract void OnEquip(Unit unit);
    public abstract void OnUnequip();
    public abstract void OnPointerEnter(Vector3 slotPosition);
    public abstract void OnPointerExit();
}
