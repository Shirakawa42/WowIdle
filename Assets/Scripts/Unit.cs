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

    public override int Level
    {
        get => (int)Stats[StatIds.Level].value;
        set => Stats[StatIds.Level].value = value;
    }

    public abstract void Tick();

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

    public void TakeDamage(float damage, DamageType damageType)
    {
        Globals.floatingDamagesManager.CreateFloatingDamage(CurrentSlot.transform.position, (int)damage, FloatingDamageType.AutoDamage, false);
        Stats[StatIds.CurrentHP].value -= damage;
        if (Stats[StatIds.CurrentHP].value <= 0)
            Die();
        UpdateBars();
    }

    public abstract void Die();

    public override void SetCurrentSlot(Slot slot)
    {
        CurrentSlot = slot;
    }

    public abstract object Clone();
}
