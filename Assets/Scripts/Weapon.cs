using UnityEngine;

public class Weapon : Equipment
{
    public int damage;
    public float cooldown;
    public float current_cooldown;

    public Weapon(string name, int itemLevel, SlotType slot, Stats stats, Sprite icon, Rarities rarity, int damage, float cooldown) : base(name, itemLevel, slot, stats, icon, rarity)
    {
        this.damage = damage;
        this.cooldown = cooldown;
        this.current_cooldown = cooldown;
    }

    public void AutoAttack(Unit target)
    {
        if (target != null) Debug.Log("Auto attacking for " + damage + " damage");
        target?.TakeDamage(damage, DamageType.Physical);
    }

    public void Tick(Unit target, Unit self)
    {
        if (current_cooldown > 0)
            current_cooldown -= Globals.tickRate;
        else
        {
            AutoAttack(target);
            current_cooldown += cooldown;
        }
    }

    public override void OnEquip(Unit unit, Slot slot)
    {
        if (SlotType == SlotType.MainHand)
        {
            equippedUnit.weaponMainhand = this;
        }
        else if (SlotType == SlotType.OffHand)
        {
            equippedUnit.weaponOffhand = this;
        }
        this.current_cooldown = cooldown;
        base.OnEquip(unit, slot);
    }

    public override void OnUnequip(Slot slot)
    {
        if (SlotType == SlotType.MainHand)
        {
            equippedUnit.weaponMainhand = null;
        }
        else if (SlotType == SlotType.OffHand)
        {
            equippedUnit.weaponOffhand = null;
        }
        base.OnUnequip(slot);
    }
}
