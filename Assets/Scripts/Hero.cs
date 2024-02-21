using System.Collections.Generic;
using UnityEngine;

public class Hero : Unit
{
    public UnitClasses unitClass;
    public UnitGears gears = new();
    public override SlotType SlotType { get; set; }
    public override Sprite Icon { get; set; }
    public override string Color { get; set; }
    public override Slot CurrentSlot { get; set; }
    public override Stats Stats { get; set; }
    public override Weapon DefaultWeapon { get; }
    public override List<Unit> TargetList { get; }
    public override bool IsActive { get; set; }

    public Hero(string name, UnitClasses unitClass)
    {
        Name = name;
        Stats = new Stats(StatsUtils.GetDefaultStatsFromUnitClass(unitClass));
        Icon = Resources.Load<Sprite>("Textures/ClassIcons/" + unitClass.ToString());
        this.unitClass = unitClass;
        SlotType = SlotType.Hero;
        Color = ColorUtils.GetColorFromClass(unitClass);
        DefaultWeapon = new("Fists", 1, SlotType.OneHand, null, Rarities.Common, new Stats(new Stat[0]), 3f, 1f, WeaponType.Mace);
        TargetList = Globals.dungeonManager.activeEnemies;
        IsActive = false;
        RecalculateUnitStats();
        RegenUnit();
    }

    public override void Tick()
    {
        PickTarget();
        Weapon mainHandWeapon = gears.GetMainHandWeapon();
        Weapon offHandWeapon = gears.GetOffHandWeapon();
        if (mainHandWeapon == null && offHandWeapon == null)
            DefaultWeapon.Tick(Target, this);
        else
        {
            mainHandWeapon?.Tick(Target, this);
            offHandWeapon?.Tick(Target, this);
        }
    }

    public override void UpdateBars()
    {
        if (CurrentSlot == null || CurrentSlot.equippedSlot == false)
            return;

        base.UpdateBars();

        ProgressBar experienceBar = CurrentSlot.extras.Find(x => x.gameObject.name == "ExperienceBar").GetComponent<ProgressBar>();
        experienceBar.UpdateValues(XpUtils.GetRequiredXp(Level), (int)Stats[StatIds.Experience].value, "#FF00CC");
    }

    public override void RecalculateUnitStats()
    {
        Stats.Recalculate();

        if (gears.GetMainHandWeapon() == null && gears.GetOffHandWeapon() == null)
        {
            Stats[StatIds.MainHandDamage].value = DefaultWeapon.damages;
            Stats[StatIds.MainHandSpeed].value = DefaultWeapon.cooldown;
            Stats[StatIds.OffHandDamage].value = 0;
            Stats[StatIds.OffHandSpeed].value = 0;
        }
        else
        {
            Stats[StatIds.MainHandDamage].value = CalcUtils.CalculateWeaponDamage(gears.GetMainHandWeapon(), this, true);
            Stats[StatIds.MainHandSpeed].value = gears.GetMainHandWeapon()?.cooldown ?? 0;
            Stats[StatIds.OffHandDamage].value = CalcUtils.CalculateWeaponDamage(gears.GetOffHandWeapon(), this, false);
            Stats[StatIds.OffHandSpeed].value = gears.GetOffHandWeapon()?.cooldown ?? 0;
        }
        UpdateBars();
    }

    public override void Die()
    {
        RegenUnit(true);
        UpdateBars();
    }

    private void LevelUp()
    {
        Stats.AddStats(new Stat[] {
            new(2, StatIds.Strength),
            new(2, StatIds.Agility),
            new(3, StatIds.Stamina),
            new(2, StatIds.Intelligence),
        });
        Level += 1;
        CurrentSlot.UpdateLevel();
        RecalculateUnitStats();
        RegenUnit();
        if (Globals.selectedHero == this)
            Globals.statsPanelManager.UpdateStats(this);
    }

    public void CheckLevelUp()
    {
        while (Level < Globals.maxLevel && Stats[StatIds.Experience].value >= XpUtils.GetRequiredXp(Level))
        {
            Stats[StatIds.Experience].value -= XpUtils.GetRequiredXp(Level);
            LevelUp();
        }
    }

    public void AddXP(int xp)
    {
        if (Level >= Globals.maxLevel)
            return;
        Stats[StatIds.Experience].value += xp;
        CheckLevelUp();
        UpdateBars();
    }

    public override void OnEquip()
    {
        IsActive = true;
        Globals.dungeonManager.activeHeroes.Add(this);
        UpdateBars();
    }

    public override void OnUnequip()
    {
        IsActive = false;
        RegenUnit();
        Globals.dungeonManager.activeHeroes.Remove(this);
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

    public override void SetCurrentSlot(Slot slot)
    {
        CurrentSlot = slot;
    }

    public override void OnClick()
    {
        if (Globals.selectedHero == this)
            return;

        Globals.selectedHero = this;
        Globals.statsPanelManager.UpdateStats(this);
        Globals.gearSlotsManager.SetGearSlots(gears, Name);
    }

    public override object Clone()
    {
        return new Hero(Name, unitClass)
        {
            Stats = Stats.Clone() as Stats,
            Icon = Icon,
            SlotType = SlotType,
            Color = Color,
            CurrentSlot = null,
            Target = null,
            gears = gears.Clone() as UnitGears
        };
    }
}
