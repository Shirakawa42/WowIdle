using UnityEngine;

public class Unit : Slotable
{
    public string unitName;
    public int level;
    public UnitStats unitStats;
    public Stats stats;
    public UnitClasses unitClass;
    public Unit target = null;
    public UnitGears gears = new();
    public Weapon defaultWeapon = new("Fists", 1, SlotType.MainHand, new Stats(0, 0, 0, 0, 0), null, Rarities.Common, 5, 1f);
    public override SlotType SlotType { get; set; }
    public override Sprite Icon { get; set; }
    public override Color Color { get; set; }
    public override Slot CurrentSlot { get; set; }

    public Unit(string unitName, int level, UnitStats unitStats, Stats stats, Sprite icon, UnitClasses unitClass)
    {
        this.unitName = unitName;
        this.level = level;
        this.unitStats = unitStats;
        this.stats = stats;
        this.Icon = icon;
        this.unitClass = unitClass;
        this.SlotType = (unitClass == UnitClasses.Enemy) ? SlotType.Enemy : SlotType.Hero;
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

        healthBar.UpdateValues(unitStats.maxHealth, unitStats.currentHealth, Color.red);
        manaBar.UpdateValues(unitStats.maxResource, unitStats.currentResource, Color.cyan);
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
        unitStats.maxHealth = 300 + level * 10 + stats.stamina * 5;
        unitStats.maxResource = 100 + level * 10 + stats.intelligence * 5;
    }

    public void RegenUnit()
    {
        unitStats.currentHealth = unitStats.maxHealth;
        unitStats.currentResource = unitStats.maxResource;
    }

    public void LevelUp()
    {
        if (unitStats.level < unitStats.maxLevel)
        {
            unitStats.level++;
            RecalculateUnitStats();
        }
    }

    public void TakeDamage(int damage, DamageType damageType)
    {
        //TODO
        Debug.Log(unitName + " took " + damage + " " + damageType + " damage");
        unitStats.currentHealth -= damage;
        if (unitStats.currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        //TODO
        Debug.Log(unitName + " died");
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

    public override void OnPointerEnter(Vector3 slotPosition)
    {
        //TODO
    }

    public override void OnPointerExit()
    {
        //TODO
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
