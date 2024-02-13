using UnityEngine;

public class Weapon : Equipment
{
    public int damage;
    public float cooldown;
    public float currentCooldown;

    public Weapon(string name, int itemLevel, SlotType slot, Stats stats, Sprite icon, Rarities rarity, int damage, float cooldown) : base(name, itemLevel, slot, stats, icon, rarity)
    {
        this.damage = damage;
        this.cooldown = cooldown;
        this.currentCooldown = cooldown;
    }

    public void AutoAttack(Unit target)
    {
        if (target != null) Debug.Log("Auto attacking for " + damage + " damage");
        target?.TakeDamage(damage, DamageType.Physical);
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
        this.currentCooldown = cooldown;
        base.OnEquip();
    }
}
