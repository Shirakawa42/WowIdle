using UnityEngine;

public class Unit : Slotable
{
    public string unitName;
    public int level;
    public UnitStats unitStats;
    public Stats stats;

    public Unit(string unitName, int level, UnitStats unitStats, Stats stats)
    {
        this.unitName = unitName;
        this.level = level;
        this.unitStats = unitStats;
        this.stats = stats;
    }

    public override SlotType SlotType { get; set; }
    public override Sprite Icon { get; set; }
    public override Color Color { get; set; }
    
    public override void OnEquip()
    {
        //TODO
    }

    public override void OnUnequip()
    {
        //TODO
    }

    public override void OnPointerEnter(Vector3 slotPosition)
    {
        //TODO
    }

    public override void OnPointerExit()
    {
        //TODO
    }
}
