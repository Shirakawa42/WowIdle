using System.Collections.Generic;
using UnityEngine;

public class Unit : Slotable
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
        set => throw new System.NotSupportedException();
    }

    public Unit(string unitName, UnitClasses unitClass)
    {
        this.unitName = unitName;
        stats = new Stats(StatsUtils.GetDefaultStatsFromUnitClass(unitClass));
        Icon = Resources.Load<Sprite>("Textures/ClassIcons/" + unitClass.ToString());
        this.unitClass = unitClass;
        SlotType = (unitClass == UnitClasses.Enemy) ? SlotType.Enemy : SlotType.Hero;
        Color = ColorUtils.GetColorFromClass(unitClass);
        RecalculateUnitStats();
        RegenUnit();
    }

    public void Tick()
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
        UpdateBars();
    }

    private void UpdateBars()
    {
        ProgressBar healthBar = CurrentSlot.extras.Find(x => x.name == "HpBar").GetComponent<ProgressBar>();
        ProgressBar manaBar = CurrentSlot.extras.Find(x => x.name == "ResourceBar").GetComponent<ProgressBar>();

        healthBar.UpdateValues((int)stats[StatIds.HP].value, (int)stats[StatIds.CurrentHP].value, "#FF0000");
        manaBar.UpdateValues((int)stats[StatIds.Mana].value, (int)stats[StatIds.CurrentMana].value, "#0000FF");
    }

    public void PickTarget()
    {
        if (unitClass == UnitClasses.Enemy)
        {
            if (Globals.activeHeroes.Count == 0)
            {
                target = null;
                return;
            }
            target = Globals.activeHeroes[Random.Range(0, Globals.activeEnemies.Count)];
        }
        else
        {
            if (Globals.activeEnemies.Count == 0)
            {
                target = null;
                return;
            }
            target = Globals.activeEnemies[Random.Range(0, Globals.activeEnemies.Count)];
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
    }

    public void RegenUnit()
    {
        stats[StatIds.CurrentHP].value = stats[StatIds.HP].value;
        stats[StatIds.CurrentMana].value = stats[StatIds.Mana].value;
    }

    public void TakeDamage(float damage, DamageType damageType)
    {
        stats[StatIds.CurrentHP].value -= damage;
        if (stats[StatIds.CurrentHP].value <= 0)
            Die();
    }

    private void Die()
    {
        //TODO
        Debug.Log(unitName + " died");
        if (unitClass == UnitClasses.Enemy)
            Globals.resourcesManager.AddMoney(Level * Random.Range(5000, 50000));
        RegenUnit();
        UpdateBars();
    }

    public override void OnEquip()
    {
        if (unitClass == UnitClasses.Enemy)
            Globals.activeEnemies.Add(this);
        else
            Globals.activeHeroes.Add(this);
        CurrentSlot.EnableExtras(true);
    }

    public override void OnUnequip()
    {
        Globals.activeHeroes.Remove(this);
        CurrentSlot.EnableExtras(false);
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
}
