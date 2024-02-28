using Unity.VisualScripting;
using UnityEngine;

public abstract class Slotable : Hoverable
{
    public abstract SlotType SlotType { get; set; }
    public abstract Sprite Icon { get; set; }
    public abstract int Level { get; set; }

    public Slot CurrentSlot { get; set; }
    public override Vector3 TooltipPosition
    {
        get => (CurrentSlot == null) ? Vector3.zero : CurrentSlot.GetTopLeftCorner();
        set => TooltipPosition = value;
    }

    public abstract void OnEquip();
    public abstract void OnUnequip();
    public virtual void OnClick() { }
    public virtual void UpdateBars() { }

    public void SetCurrentSlot(Slot slot)
    {
        CurrentSlot = slot;
    }

}
