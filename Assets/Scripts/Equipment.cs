using System;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Slotable, ICloneable
{
    public string equipmentName;
    public Stats stats;
    public Rarities rarity;
    public Hero equippedUnit;
    public GearMat gearMat;

    public override SlotType SlotType { get; set; }
    public override Sprite Icon { get; set; }
    public override string Color { get; set; }
    public override Slot CurrentSlot { get; set; }
    public override int Level { get; set; }

    public Equipment(string name, int itemLevel, SlotType slot, Sprite icon, Rarities rarity, Stats stats, GearMat gearMat = GearMat.None)
    {
        equipmentName = name;
        Level = itemLevel;
        Icon = icon;
        this.rarity = rarity;
        SlotType = slot;
        Color = ColorUtils.GetColorFromRarity(rarity);
        this.stats = stats;
        this.gearMat = gearMat;
    }

    public virtual List<TooltipValue> GetTooltipValues()
    {
        string gearType = SlotType.ToString();
        if (gearMat != GearMat.None)
            gearType += " - " + gearMat.ToString();

        List<TooltipValue> tooltipValues = new()
        {
            new TooltipValue(equipmentName, "", ValueType.Name, rarity),
            new TooltipValue(gearType, "", ValueType.EquipmentType, rarity)
        };
        foreach (Stat stat in stats.GetUsedStats())
            tooltipValues.Add(new TooltipValue(stat.name, stat.value.ToString(), stat.type, rarity));
        return tooltipValues;
    }

    public override void OnEquip()
    {
        if (equippedUnit != null) return;
        equippedUnit = Globals.selectedHero;
        equippedUnit.gears.Equip(CurrentSlot.id, this);
        equippedUnit.Stats.AddStats(stats.GetUsedStats());
        equippedUnit.RecalculateUnitStats();
        equippedUnit.RegenUnit();
        Globals.statsPanelManager.UpdateStats(equippedUnit);
    }

    public override void OnUnequip()
    {
        if (equippedUnit == null) return;
        equippedUnit.Stats.RemoveStats(stats.GetUsedStats());
        equippedUnit.gears.Unequip(CurrentSlot.id);
        equippedUnit.RecalculateUnitStats();
        equippedUnit.RegenUnit();
        Globals.statsPanelManager.UpdateStats(equippedUnit);
        equippedUnit = null;
    }

    public override void OnPointerEnter()
    {
        Globals.itemTooltipManager.ShowTooltip(GetTooltipValues(), Color, CurrentSlot.GetTopLeftCorner());
    }

    public override void OnPointerExit()
    {
        Globals.itemTooltipManager.HideTooltip();
    }

    public override void SetCurrentSlot(Slot slot)
    {
        CurrentSlot = slot;
    }

    public object Clone()
    {
        return new Equipment(equipmentName, Level, SlotType, Icon, rarity, (Stats)stats.Clone(), gearMat);
    }
}
