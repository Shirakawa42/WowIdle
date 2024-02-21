using System;
using System.Collections.Generic;
using System.Linq;

public static class StatsUtils
{
    public const int maxLevel = 60;
    public const int baseHP = 100;
    public const int baseMana = 50;

    public static string GetStatNameFromStatId(StatIds id)
    {
        switch (id)
        {
            case StatIds.HP:
                return "HP";
            case StatIds.CurrentHP:
                return "Current HP";
            case StatIds.Mana:
                return "Mana";
            case StatIds.CurrentMana:
                return "Current Mana";
            case StatIds.Level:
                return "Level";
            case StatIds.maxLevel:
                return "Max Level";
            case StatIds.Experience:
                return "Experience";
            case StatIds.Strength:
                return "Strength";
            case StatIds.Agility:
                return "Agility";
            case StatIds.Intelligence:
                return "Intelligence";
            case StatIds.Stamina:
                return "Stamina";
            case StatIds.Armor:
                return "Armor";
            case StatIds.MainHandDamage:
                return "Main Hand Damages";
            case StatIds.MainHandSpeed:
                return "Main Hand Speed";
            case StatIds.OffHandDamage:
                return "Off Hand Damages";
            case StatIds.OffHandSpeed:
                return "Off Hand Speed";
            case StatIds.CritChances:
                return "Crit Chances";
            case StatIds.CritDamage:
                return "Crit Damages";
            case StatIds.HitChances:
                return "Hit Chances";
            case StatIds.DodgeChances:
                return "Dodge Chances";
            case StatIds.ParryChances:
                return "Parry Chances";
            case StatIds.BlockChances:
                return "Block Chances";
            case StatIds.TotalPhysicalReduction:
                return "Physical Reduction";
            case StatIds.ArmorPenetration:
                return "Armor Penetration";
            default:
                throw new Exception("UNKNOWN");
        }
    }

    public static Stat[] GetDefaultStatsFromUnitClass(UnitClasses unitclass)
    {
        List<Stat> everyone = new()
        {
            new(baseHP, StatIds.HP),
            new(baseMana, StatIds.Mana),
            new(baseHP, StatIds.CurrentHP),
            new(baseMana, StatIds.CurrentMana),
            new(maxLevel, StatIds.maxLevel),
            new(1, StatIds.Level),
            new(85, StatIds.HitChances),
            new(5, StatIds.CritChances),
            new(150, StatIds.CritDamage),
            new(5, StatIds.DodgeChances),
        };

        List<Stat> classStats = new();

        if (unitclass == UnitClasses.Mage)
            classStats = new()
            {
                new(4, StatIds.Intelligence),
                new(2, StatIds.Strength),
                new(2, StatIds.Agility),
                new(3, StatIds.Stamina),
            };
        else if (unitclass == UnitClasses.Rogue)
            classStats = new()
            {
                new(2, StatIds.Intelligence),
                new(2, StatIds.Strength),
                new(4, StatIds.Agility),
                new(4, StatIds.Stamina),
            };
        else if (unitclass == UnitClasses.Shaman)
            classStats = new()
            {
                new(3, StatIds.Intelligence),
                new(3, StatIds.Strength),
                new(3, StatIds.Agility),
                new(4, StatIds.Stamina),
            };
        return everyone.Concat(classStats).ToArray();
    }

    public static ValueType GetValueTypeFromStatId(StatIds id)
    {
        return id switch
        {
            StatIds.HP => ValueType.HP,
            StatIds.Mana => ValueType.Mana,
            StatIds.Level => ValueType.Level,
            StatIds.Strength => ValueType.MainStat,
            StatIds.Agility => ValueType.MainStat,
            StatIds.Intelligence => ValueType.MainStat,
            StatIds.Stamina => ValueType.Stamina,
            StatIds.Armor => ValueType.Stamina,
            StatIds.Experience => ValueType.Invisible,
            StatIds.CurrentHP => ValueType.Invisible,
            StatIds.CurrentMana => ValueType.Invisible,
            StatIds.maxLevel => ValueType.Invisible,
            StatIds.MainHandDamage => ValueType.Weapon,
            StatIds.MainHandSpeed => ValueType.Weapon,
            StatIds.OffHandDamage => ValueType.Weapon,
            StatIds.OffHandSpeed => ValueType.Weapon,
            StatIds.CritChances => ValueType.SecondaryStatPercent,
            StatIds.CritDamage => ValueType.SecondaryStatPercent,
            StatIds.HitChances => ValueType.SecondaryStatPercent,
            StatIds.DodgeChances => ValueType.SecondaryStatPercent,
            StatIds.ParryChances => ValueType.SecondaryStatPercent,
            StatIds.BlockChances => ValueType.SecondaryStatPercent,
            StatIds.TotalPhysicalReduction => ValueType.SecondaryStatPercent,
            StatIds.ArmorPenetration => ValueType.SecondaryStat,
            _ => ValueType.Invisible,
        };
    }
}

public class Stat : ICloneable
{
    public string name;
    public float value;
    public ValueType type;
    public StatIds id;

    public Stat(float value, StatIds id)
    {
        this.name = StatsUtils.GetStatNameFromStatId(id);
        this.value = value;
        this.type = StatsUtils.GetValueTypeFromStatId(id);
        this.id = id;
    }

    public static Stat operator +(Stat a, Stat b)
    {
        if (a.id != b.id)
            throw new Exception("You can't add two stats with different names");

        return new Stat(a.value + b.value, a.id);
    }

    public static Stat operator -(Stat a, Stat b)
    {
        if (a.id != b.id)
            throw new Exception("You can't subtract two stats with different names");

        return new Stat(a.value - b.value, a.id);
    }

    public object Clone()
    {
        return new Stat(value, id);
    }
}