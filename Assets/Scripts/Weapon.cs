using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    private float currentCooldown;
    public float damages;
    public float cooldown;

    public Weapon(string name, int itemLevel, SlotType slot, Sprite icon, Rarities rarity, Stats stats, float damages, float cooldown) : base(name, itemLevel, slot, icon, rarity, stats)
    {
        this.damages = damages;
        this.cooldown = cooldown;
        currentCooldown = cooldown;
    }

    public void AutoAttack(Unit target)
    {
        target?.TakeDamage(damages, DamageType.Physical);
    }

    public void Tick(Unit target, Unit self)
    {
        if (currentCooldown > 0)
            currentCooldown -= Globals.tickRate;
        else
        {
            AutoAttack(target);
            currentCooldown += cooldown;
        }
    }

    public override void OnEquip()
    {
        currentCooldown = cooldown;
        base.OnEquip();
    }

    public override List<TooltipValue> GetTooltipValues()
    {
        List<TooltipValue> tooltipValues = new()
        {
            new TooltipValue(equipmentName, "", ValueType.Name, rarity),
            new TooltipValue(SlotType.ToString(), "", ValueType.EquipmentType, rarity),
            new TooltipValue("Damages", damages.ToString(), ValueType.Armor, rarity),
            new TooltipValue("Speed", cooldown.ToString(), ValueType.Armor, rarity)
        };
        foreach (Stat stat in stats.GetUsedStats())
            tooltipValues.Add(new TooltipValue(stat.name, stat.value.ToString(), stat.type, rarity));
        return tooltipValues;
    }
}
