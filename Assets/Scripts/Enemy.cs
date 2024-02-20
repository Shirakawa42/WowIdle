using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public bool isBoss;
    private readonly int droppedMoney;
    public readonly int droppedExp;

    public override SlotType SlotType { get; set; }
    public override Sprite Icon { get; set; }
    public override string Color { get; set; }
    public override Slot CurrentSlot { get; set; }
    public override Stats Stats { get; set; }
    public override Weapon DefaultWeapon { get; }
    public override List<Unit> TargetList { get; }
    public override bool IsActive { get; set; }

    public Enemy(string name, bool isBoss, string icon, int level, float weaponDamages, float weaponSpeed, Stat[] stats)
    {
        Name = name;
        this.isBoss = isBoss;
        Stats = new Stats(stats);
        Level = level;
        Icon = Resources.Load<Sprite>("Textures/Enemies/" + icon);

        droppedMoney = Level + Random.Range(1, 3);
        droppedExp = Level * 2;
        if (isBoss)
        {
            droppedMoney *= 5;
            droppedExp *= 3;
        }
        DefaultWeapon = new("Fists", 1, SlotType.OneHand, null, Rarities.Common, new Stats(new Stat[0]), weaponDamages, weaponSpeed, WeaponType.Mace);
        TargetList = Globals.dungeonManager.activeHeroes;
        SlotType = SlotType.Enemy;
        Color = "#FF0000";
        IsActive = false;
        RegenUnit();
    }

    public override void Tick()
    {
        PickTarget();
        DefaultWeapon.Tick(Target, this);
    }

    public override void Die()
    {
        Globals.resourcesManager.AddMoney(droppedMoney);
        Globals.dungeonManager.OnEnemyDeath(this);
    }

    public override void OnEquip()
    {
        IsActive = true;
        Globals.dungeonManager.activeEnemies.Add(this);
        UpdateBars();
    }

    public override void OnUnequip()
    {
        IsActive = false;
        Globals.dungeonManager.activeEnemies.Remove(this);
    }

    public void Remove()
    {
        CurrentSlot.EmptySlot();
        CurrentSlot = null;
    }

    public override object Clone()
    {
        return new Enemy(Name, isBoss, Icon.name, Level, DefaultWeapon.damages, DefaultWeapon.cooldown, Stats.GetClonedStats());
    }

    public override void OnPointerEnter()
    {
        List<TooltipValue> tooltipValues = new()
        {
            new TooltipValue(Name, "", ValueType.Name),
        };
        Globals.itemTooltipManager.ShowTooltip(tooltipValues, Color, CurrentSlot.GetTopLeftCorner());
    }

    public override void OnPointerExit()
    {
        Globals.itemTooltipManager.HideTooltip();
    }
}