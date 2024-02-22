using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    private float currentCooldown;
    public float damages;
    public float cooldown;
    public WeaponType weaponType;
    private float calculatedDamages;

    public Weapon(string name, int itemLevel, SlotType slot, Sprite icon, Rarities rarity, Stats stats, float damages, float cooldown, WeaponType weaponType) : base(name, itemLevel, slot, icon, rarity, stats)
    {
        this.damages = damages;
        this.cooldown = cooldown;
        currentCooldown = cooldown;
        this.weaponType = weaponType;
    }

    public void AutoAttack(Unit target, Unit self)
    {
        target?.TakeDamage(calculatedDamages, DamageType.Physical, self.Stats);
    }

    public void SetCalculatedDamages(float damages)
    {
        calculatedDamages = damages;
    }

    public void Tick(Unit target, Unit self)
    {
        if (currentCooldown > 0)
            currentCooldown -= Globals.tickRate;
        else
        {
            currentCooldown += cooldown;
            if (target == null)
                return;
            self.CurrentSlot.PlayAttackAnimation(target.CurrentSlot.transform.position);
            AutoAttack(target, self);
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
            new TooltipValue(SlotType.ToString() + " " + weaponType.ToString(), "", ValueType.EquipmentType, rarity),
            new TooltipValue("Damages", damages.ToString(), ValueType.Armor, rarity),
            new TooltipValue("Speed", cooldown.ToString(), ValueType.Armor, rarity)
        };
        foreach (Stat stat in stats.GetUsedStats())
            tooltipValues.Add(new TooltipValue(stat.name, stat.value.ToString(), stat.type, rarity));
        return tooltipValues;
    }
}
