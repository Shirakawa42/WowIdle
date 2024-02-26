using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Unit : Slotable, ICloneable
{
    public string Name;
    public Unit Target { get; set; }
    public abstract Stats Stats { get; set; }
    public abstract Weapon DefaultWeapon { get; }
    public abstract List<Unit> TargetList { get; }
    public abstract bool IsActive { get; set; }

    public readonly List<Behavior> behaviors = new();

    public override int Level
    {
        get => (int)Stats[StatIds.Level].value;
        set => Stats[StatIds.Level].value = value;
    }

    public virtual void Tick()
    {
        List<Behavior> toRemove = new();

        foreach (Behavior behavior in behaviors)
        {
            if (!behavior.Tick())
                toRemove.Add(behavior);
        }
        foreach (Behavior behavior in toRemove)
            RemoveBehavior(behavior);
        if (CurrentSlot != null && CurrentSlot.equippedSlot)
            CurrentSlot.Refresh();
    }

    public override void UpdateBars()
    {
        if (CurrentSlot == null || CurrentSlot.equippedSlot == false)
            return;

        if (CurrentSlot.GetSlotable() != this)
            Debug.LogError("CurrentSlot.GetSlotable() != this");

        ProgressBar healthBar = CurrentSlot.extras.Find(x => x.gameObject.name == "HpBar").GetComponent<ProgressBar>();
        ProgressBar manaBar = CurrentSlot.extras.Find(x => x.gameObject.name == "ResourceBar").GetComponent<ProgressBar>();

        healthBar.UpdateValues((int)Stats[StatIds.HP].value, (int)Stats[StatIds.CurrentHP].value, "#FF0000");
        manaBar.UpdateValues((int)Stats[StatIds.Mana].value, (int)Stats[StatIds.CurrentMana].value, "#0000FF");
    }

    public void PickTarget()
    {
        if (TargetList.Count == 0)
        {
            Target = null;
            return;
        }
        Target = TargetList[Random.Range(0, TargetList.Count)];
    }

    public void RegenUnit(bool force = false)
    {
        if (Stats[StatIds.CurrentHP].value > Stats[StatIds.HP].value)
            Stats[StatIds.CurrentHP].value = Stats[StatIds.HP].value;
        if (Stats[StatIds.CurrentMana].value > Stats[StatIds.Mana].value)
            Stats[StatIds.CurrentMana].value = Stats[StatIds.Mana].value;
        if (!force && IsActive == true)
        {
            UpdateBars();
            return;
        }
        Stats[StatIds.CurrentHP].value = Stats[StatIds.HP].value;
        Stats[StatIds.CurrentMana].value = Stats[StatIds.Mana].value;
        UpdateBars();
    }

    public void TakeDamage(float damage, DamageType damageType, Stats casterStats)
    {
        int casterLevel = (int)casterStats[StatIds.Level].value;
        int level = (int)Stats[StatIds.Level].value;

        //loose 5% crit chance per level difference
        float critChance = casterStats[StatIds.CritChances].value - (level - casterLevel) * 5f;
        //loose 5% hit chance per level difference
        float hitChance = casterStats[StatIds.HitChances].value - (level - casterLevel) * 5f;

        bool isCrit = Random.Range(0f, 100f) < critChance;
        bool isHit = Random.Range(0f, 100f) < hitChance;

        if (!isHit)
        {
            Globals.floatingDamagesManager.CreateFloatingDamage(CurrentSlot.transform.position, 0, FloatingDamageType.Miss, false);
            return;
        }
        if (isCrit)
            damage *= casterStats[StatIds.CritDamage].value / 100f;

        if (damageType == DamageType.Physical)
        {
            float armor = Mathf.Max(Stats[StatIds.Armor].value - casterStats[StatIds.ArmorPenetration].value, 0f);
            float armorReduction = CalcUtils.GetArmorReduction(armor, casterStats[StatIds.Level].value);
            damage *= 1f - (armorReduction / 100f);
        }

        Globals.floatingDamagesManager.CreateFloatingDamage(CurrentSlot.transform.position, Mathf.RoundToInt(damage), FloatingDamageType.AutoDamage, isCrit);
        Stats[StatIds.CurrentHP].value -= damage;
        if (Stats[StatIds.CurrentHP].value <= 0)
            Die();
        UpdateBars();
    }

    public void AddBehavior(Behavior behavior)
    {
        behaviors.Add(behavior);
    }

    public void RemoveBehavior(Behavior behavior)
    {
        behavior.OnRemove();
        behaviors.Remove(behavior);
    }

    public abstract void Die();

    public abstract object Clone();

    public abstract void RecalculateUnitStats();
}
