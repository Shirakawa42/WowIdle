using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Unit : Slotable, ICloneable
{
    public string unitName;
    public Stats stats;
    public UnitClasses unitClass;
    public Unit target = null;
    public UnitGears gears = new();
    public Weapon defaultWeapon = new("Fists", 1, SlotType.OneHand, null, Rarities.Common, new Stats(new Stat[0]), 1f, 1f, WeaponType.Mace);
    public override SlotType SlotType { get; set; }
    public override Sprite Icon { get; set; }
    public override string Color { get; set; }
    public override Slot CurrentSlot { get; set; }

    public override int Level
    {
        get => (int)stats[StatIds.Level].value;
        set => stats[StatIds.Level].value = value;
    }

    public Unit(string unitName, UnitClasses unitClass)
    {
        this.unitName = unitName;
        stats = new Stats(StatsUtils.GetDefaultStatsFromUnitClass(unitClass));
        if (unitClass != UnitClasses.Enemy)
            Icon = Resources.Load<Sprite>("Textures/ClassIcons/" + unitClass.ToString());
        this.unitClass = unitClass;
        SlotType = (unitClass == UnitClasses.Enemy) ? SlotType.Enemy : SlotType.Hero;
        Color = ColorUtils.GetColorFromClass(unitClass);
        RecalculateUnitStats();
        RegenUnit();
    }

    public virtual void Tick()
    {
        PickTarget();
        Weapon mainHandWeapon = gears.GetMainHandWeapon();
        Weapon offHandWeapon = gears.GetOffHandWeapon();
        if (mainHandWeapon == null && offHandWeapon == null)
            defaultWeapon.Tick(target, this);
        else
        {
            mainHandWeapon?.Tick(target, this);
            offHandWeapon?.Tick(target, this);
        }
    }

    public void UpdateBars()
    {
        if (CurrentSlot == null || CurrentSlot.equippedSlot == false)
            return;

        ProgressBar healthBar = CurrentSlot.extras.Find(x => x.gameObject.name == "HpBar").GetComponent<ProgressBar>();
        ProgressBar manaBar = CurrentSlot.extras.Find(x => x.gameObject.name == "ResourceBar").GetComponent<ProgressBar>();

        healthBar.UpdateValues((int)stats[StatIds.HP].value, (int)stats[StatIds.CurrentHP].value, "#FF0000");
        manaBar.UpdateValues((int)stats[StatIds.Mana].value, (int)stats[StatIds.CurrentMana].value, "#0000FF");

        if (unitClass == UnitClasses.Enemy)
            return;
        ProgressBar experienceBar = CurrentSlot.extras.Find(x => x.gameObject.name == "ExperienceBar").GetComponent<ProgressBar>();
        experienceBar.UpdateValues(XpUtils.GetRequiredXp(Level), (int)stats[StatIds.Experience].value, "#FF00CC");
    }

    public void PickTarget()
    {
        if (unitClass == UnitClasses.Enemy)
        {
            if (Globals.dungeonManager.activeHeroes.Count == 0)
            {
                target = null;
                return;
            }
            target = Globals.dungeonManager.activeHeroes[Random.Range(0, Globals.dungeonManager.activeHeroes.Count)];
        }
        else
        {
            if (Globals.dungeonManager.activeEnemies.Count == 0)
            {
                target = null;
                return;
            }
            target = Globals.dungeonManager.activeEnemies[Random.Range(0, Globals.dungeonManager.activeEnemies.Count)];
        }
    }

    public void RecalculateUnitStats()
    {
        stats[StatIds.HP].value = StatsUtils.baseHP + stats[StatIds.Stamina].value * 10;
        stats[StatIds.Mana].value = StatsUtils.baseMana + stats[StatIds.Intelligence].value * 10;
        stats[StatIds.CurrentHP].value = stats[StatIds.HP].value;
        stats[StatIds.CurrentMana].value = stats[StatIds.Mana].value;

        if (gears.GetMainHandWeapon() == null && gears.GetOffHandWeapon() == null)
        {
            stats[StatIds.MainHandDamage].value = defaultWeapon.damages;
            stats[StatIds.MainHandSpeed].value = defaultWeapon.cooldown;
            stats[StatIds.OffHandDamage].value = 0;
            stats[StatIds.OffHandSpeed].value = 0;
        }
        else
        {
            stats[StatIds.MainHandDamage].value = CalcUtils.CalculateWeaponDamage(gears.GetMainHandWeapon(), this, true);
            stats[StatIds.MainHandSpeed].value = gears.GetMainHandWeapon()?.cooldown ?? 0;
            stats[StatIds.OffHandDamage].value = CalcUtils.CalculateWeaponDamage(gears.GetOffHandWeapon(), this, false);
            stats[StatIds.OffHandSpeed].value = gears.GetOffHandWeapon()?.cooldown ?? 0;
        }
        UpdateBars();
    }

    public void RegenUnit()
    {
        stats[StatIds.CurrentHP].value = stats[StatIds.HP].value;
        stats[StatIds.CurrentMana].value = stats[StatIds.Mana].value;
        UpdateBars();
    }

    public void TakeDamage(float damage, DamageType damageType)
    {
        stats[StatIds.CurrentHP].value -= damage;
        if (stats[StatIds.CurrentHP].value <= 0)
            Die();
        UpdateBars();
    }

    public virtual void Die()
    {
        RegenUnit();
        UpdateBars();
    }

    private void LevelUp()
    {
        stats.AddStats(new Stat[] {
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
        while (stats[StatIds.Experience].value >= XpUtils.GetRequiredXp(Level))
        {
            stats[StatIds.Experience].value -= XpUtils.GetRequiredXp(Level);
            LevelUp();
        }
    }

    public void AddXP(int xp)
    {
        if (Level >= Globals.maxLevel)
            return;
        stats[StatIds.Experience].value += xp;
        CheckLevelUp();
        UpdateBars();
    }

    public override void OnEquip()
    {
        if (unitClass == UnitClasses.Enemy)
            Globals.dungeonManager.activeEnemies.Add(this);
        else
            Globals.dungeonManager.activeHeroes.Add(this);
        UpdateBars();
    }

    public override void OnUnequip()
    {
        Globals.dungeonManager.activeHeroes.Remove(this);
    }

    public override void OnPointerEnter()
    {
        List<TooltipValue> tooltipValues = new()
        {
            new TooltipValue(unitName, "", ValueType.Name),
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
        Globals.gearSlotsManager.SetGearSlots(gears, unitName);
    }

    public virtual object Clone()
    {
        return new Unit(unitName, unitClass)
        {
            stats = stats.Clone() as Stats,
            Icon = Icon,
            SlotType = SlotType,
            Color = Color,
            CurrentSlot = null,
            target = null,
            gears = gears.Clone() as UnitGears
        };
    }
}
