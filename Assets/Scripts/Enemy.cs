using UnityEngine;

public class Enemy : Unit
{
    public bool isBoss;

    private readonly int droppedMoney;
    public readonly int droppedExp;

    public Enemy(string name, bool isBoss, string icon, int level, float weaponDamages, float weaponSpeed, Stat[] stats) : base(name, UnitClasses.Enemy)
    {
        this.isBoss = isBoss;
        this.stats.AddStats(stats);
        this.stats[StatIds.Level] = new Stat(level, StatIds.Level);
        Icon = Resources.Load<Sprite>("Textures/Enemies/" + icon);

        int HpResources = level * 5;
        if (isBoss)
            HpResources *= 2;

        this.stats[StatIds.HP].value += HpResources;
        this.stats[StatIds.CurrentHP].value += HpResources;
        this.stats[StatIds.Mana].value += HpResources;
        this.stats[StatIds.CurrentMana].value += HpResources; 

        droppedMoney = Level + Random.Range(1, 3);
        droppedExp = Level * 2;
        if (isBoss)
        {
            droppedMoney *= 5;
            droppedExp *= 3;
        }
        defaultWeapon = new("Fists", 1, SlotType.OneHand, null, Rarities.Common, new Stats(new Stat[0]), weaponDamages, weaponSpeed, WeaponType.Mace);
    }

    public override void Tick()
    {
        PickTarget();
        defaultWeapon.Tick(target, this);
    }

    public override void Die()
    {
        Globals.resourcesManager.AddMoney(droppedMoney);
        Globals.dungeonManager.OnEnemyDeath(this);
    }

    public void Remove()
    {
        CurrentSlot.EmptySlot();
    }

    public override object Clone()
    {
        return new Enemy(unitName, isBoss, Icon.name, Level, defaultWeapon.damages, defaultWeapon.cooldown, stats.GetUsedStats());
    }
}   