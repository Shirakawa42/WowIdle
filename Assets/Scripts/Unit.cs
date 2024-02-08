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
        RecalculateUnitStats();
    }

    public override SlotType SlotType { get; set; }
    public override Sprite Icon { get; set; }
    public override Color Color { get; set; }

    public void RecalculateUnitStats()
    {
        unitStats.maxHealth = 100 + level * 10 + stats.stamina * 5;
        unitStats.maxResource = 100 + level * 10 + stats.intelligence * 5;
    }
    
    public override void OnEquip(Unit unit)
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
