using System;
using UnityEngine;

public class Stats : ICloneable
{
    private readonly Stat[] stats;

    public Stats()
    {
        stats = new Stat[Enum.GetNames(typeof(StatIds)).Length];
        for (int i = 0; i < stats.Length; i++)
            stats[i] = new Stat(0, (StatIds)i);
    }

    public Stats(Stat[] defaultStats)
    {
        stats = new Stat[Enum.GetNames(typeof(StatIds)).Length];
        for (int i = 0; i < stats.Length; i++)
            stats[i] = new Stat(0, (StatIds)i);

        foreach (Stat stat in defaultStats)
            stats[(int)stat.id] = stat;
    }

    public Stat[] GetAllStats()
    {
        return stats;
    }

    public Stat[] GetUsedStats()
    {
        return Array.FindAll(stats, s => s.value != 0);
    }

    public Stat[] GetClonedStats()
    {
        Stat[] usedStats = GetUsedStats();
        Stat[] clonedStats = new Stat[usedStats.Length];
        for (int i = 0; i < usedStats.Length; i++)
            clonedStats[i] = (Stat)usedStats[i].Clone();
        return clonedStats;
    }

    public Stat this[StatIds index]
    {
        get => stats[(int)index];
        set => stats[(int)index] = value;
    }

    public void AddStats(Stat[] stats)
    {
        foreach (Stat stat in stats)
            this[stat.id] += stat;
    }

    public void RemoveStats(Stat[] stats)
    {
        foreach (Stat stat in stats)
            this[stat.id] -= stat;
    }

    public void Recalculate()
    {
        this[StatIds.HP].value = StatsUtils.baseHP + this[StatIds.Stamina].value * 10;
        this[StatIds.Mana].value = StatsUtils.baseMana + this[StatIds.Intelligence].value * 5;
        this[StatIds.TotalPhysicalReduction].value = CalcUtils.GetArmorReduction(this[StatIds.Armor].value, this[StatIds.Level].value);
    }

    public object Clone()
    {
        return new Stats(GetClonedStats());
    }

}
