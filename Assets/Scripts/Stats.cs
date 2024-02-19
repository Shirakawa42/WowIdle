using System;

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

    public object Clone()
    {
        return new Stats(GetUsedStats());
    }

}
