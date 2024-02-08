using System.Collections.Generic;
using UnityEngine;

public class Equipment : Slotable
{
    public string equipmentName;
    public int itemLevel;
    public Stats stats;
    public Rarities rarity;
    public Unit equippedUnit;

    public override SlotType SlotType { get; set; }
    public override Sprite Icon { get; set; }
    public override Color Color { get; set; }

    public Equipment(string name, int itemLevel, SlotType slot, Stats stats, Sprite icon, Rarities rarity)
    {
        this.equipmentName = name;
        this.itemLevel = itemLevel;
        this.stats = stats;
        this.Icon = icon;
        this.rarity = rarity;
        this.SlotType = slot;
        this.Color = ItemUtils.GetRarityColor(rarity);
    }

    public List<TooltipValue> GetTooltipValues()
    {
        List<TooltipValue> tooltipValues = new();
        if (stats.strength > 0) tooltipValues.Add(new TooltipValue("Strength", stats.strength.ToString(), stats.GetStatColor("Strength")));
        if (stats.agility > 0) tooltipValues.Add(new TooltipValue("Agility", stats.agility.ToString(), stats.GetStatColor("Agility")));
        if (stats.intelligence > 0) tooltipValues.Add(new TooltipValue("Intelligence", stats.intelligence.ToString(), stats.GetStatColor("Intelligence")));
        if (stats.stamina > 0) tooltipValues.Add(new TooltipValue("Stamina", stats.stamina.ToString(), stats.GetStatColor("Stamina")));
        if (stats.armor > 0) tooltipValues.Add(new TooltipValue("Armor", stats.armor.ToString(), stats.GetStatColor("Armor")));

        return tooltipValues;
    }

    public override void OnEquip(Unit unit)
    {
        Debug.Log("Equipping " + equipmentName + " to " + unit.unitName);
        equippedUnit = unit;
        equippedUnit.stats += stats;
        equippedUnit.RecalculateUnitStats();
        Globals.statsPanelManager.UpdateStats(equippedUnit);
    }

    public override void OnUnequip()
    {
        equippedUnit.stats -= stats;
        equippedUnit.RecalculateUnitStats();
        Globals.statsPanelManager.UpdateStats(equippedUnit);
        equippedUnit = null;
    }

    public override void OnPointerEnter(Vector3 slotPosition)
    {
        Globals.itemTooltipManager.ShowTooltip(GetTooltipValues(), Color, slotPosition + new Vector3(-272/2, 175/2, 0), equipmentName, Color);
    }

    public override void OnPointerExit()
    {
        Globals.itemTooltipManager.HideTooltip();
    }
}
