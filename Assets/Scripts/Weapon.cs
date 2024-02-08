using UnityEngine;

public class Weapon : Equipment
{
    public int damage;
    public float cooldown;

    public Weapon(string name, int itemLevel, SlotType slot, Stats stats, Sprite icon, Rarities rarity, int damage, float cooldown) : base(name, itemLevel, slot, stats, icon, rarity)
    {
        this.damage = damage;
        this.cooldown = cooldown;
    }
}
