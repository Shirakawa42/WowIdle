using System.Collections.Generic;
using UnityEngine;

public class Equipment : Slotable
{
    public string equipmentName;
    public Stats stats;
    public Rarities rarity;
    public Unit equippedUnit;

    public override SlotType SlotType { get; set; }
    public override Sprite Icon { get; set; }
    public override Color Color { get; set; }
    public override Slot CurrentSlot { get; set; }
    public override int Level { get; set; }

    public Equipment(string name, int itemLevel, SlotType slot, Stats stats, Sprite icon, Rarities rarity)
    {
        this.equipmentName = name;
        this.Level = itemLevel;
        this.stats = stats;
        this.Icon = icon;
        this.rarity = rarity;
        this.SlotType = slot;
        this.Color = ColorUtils.GetColorFromRarity(rarity);
    }

    public List<TooltipValue> GetTooltipValues()
    {
        List<TooltipValue> tooltipValues = new()
        {
            new TooltipValue(equipmentName, "", TooltipValueType.Name, rarity),
            new TooltipValue(SlotType.ToString(), "", TooltipValueType.EquipmentType, rarity)
        };
        if (stats.armor > 0) tooltipValues.Add(new TooltipValue("Armor", stats.armor.ToString(), TooltipValueType.Armor));
        if (stats.strength > 0) tooltipValues.Add(new TooltipValue("Strength", stats.strength.ToString(), TooltipValueType.MainStat));
        if (stats.agility > 0) tooltipValues.Add(new TooltipValue("Agility", stats.agility.ToString(), TooltipValueType.MainStat));
        if (stats.intelligence > 0) tooltipValues.Add(new TooltipValue("Intelligence", stats.intelligence.ToString(), TooltipValueType.MainStat));
        if (stats.stamina > 0) tooltipValues.Add(new TooltipValue("Stamina", stats.stamina.ToString(), TooltipValueType.MainStat));
        return tooltipValues;
    }

    public override void OnEquip()
    {
        if (equippedUnit != null) return;
        equippedUnit = Globals.selectedHero;
        equippedUnit.gears.Equip(CurrentSlot.id, this);
        equippedUnit.stats += stats;
        equippedUnit.RecalculateUnitStats();
        Globals.statsPanelManager.UpdateStats(equippedUnit);
    }

    public override void OnUnequip()
    {
        if (equippedUnit == null) return;
        equippedUnit.stats -= stats;
        equippedUnit.gears.Unequip(CurrentSlot.id);
        equippedUnit.RecalculateUnitStats();
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
}
