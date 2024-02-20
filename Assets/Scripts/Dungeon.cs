using System.Collections.Generic;

public struct Floor
{
    public List<string> enemies;

    public Floor(List<string> enemies)
    {
        this.enemies = enemies;
    }
}

public struct LootProbabilities
{
    public float nothing;
    public float poor;
    public float common;
    public float uncommon;
    public float rare;
    public float epic;
    public float legendary;

    public LootProbabilities(float nothing = 100f, float poor = 0f, float common = 0f, float uncommon = 0f, float rare = 0f, float epic = 0f, float legendary = 0f) // probabilities should add up to 100
    {
        this.nothing = nothing;
        this.poor = poor;
        this.common = common;
        this.uncommon = uncommon;
        this.rare = rare;
        this.epic = epic;
        this.legendary = legendary;
    }
}

public struct LevelRange
{
    public int min;
    public int max;

    public LevelRange(int min, int max)
    {
        this.min = min;
        this.max = max;
    }
}

public abstract class Dungeon
{
    public abstract List<Floor> Floors { get; }
    public abstract string Name { get; }
    public abstract Dictionary<string, Enemy> EnemyTypes { get; }
    public abstract LevelRange LootLevelRange { get; }
    public abstract LootProbabilities LootProbabilities { get; }
    public abstract LootProbabilities BossLootProbabilities { get; }
}