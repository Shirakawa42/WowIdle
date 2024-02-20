using System.Collections.Generic;

public class GreenForest : Dungeon
{
    public override string Name { get; } = "Green Forest";

    public override Dictionary<string, Enemy> EnemyTypes { get; } = new()
    {
        { "Kobold", new("Kobold", false, "Kobold", 1, 5f, 2f, new Stat[0]) },
        { "Adept", new("Adept Kobold", false, "Kobold", 3, 10f, 2f, new Stat[0]) },
        { "Boss", new("Kobold Boss", true, "Kobold", 5, 15f, 2f, new Stat[0]) },
    };

    public override List<Floor> Floors { get; } = new()
    {
        new(new List<string> { "Kobold" }),
        new(new List<string> { "Kobold" }),
        new(new List<string> { "Kobold", "Kobold" }),
        new(new List<string> { "Kobold", "Kobold" }),
        new(new List<string> { "Adept" }),
        new(new List<string> { "Adept", "Kobold", "Kobold" }),
        new(new List<string> { "Adept", "Adept", "Kobold" }),
        new(new List<string> { "Adept", "Adept", "Kobold", "Kobold", "Kobold" }),
        new(new List<string> { "Adept", "Adept", "Adept", "Kobold", "Kobold", "Kobold" }),
        new(new List<string> { "Adept", "Adept", "Adept", "Kobold", "Kobold", "Kobold", "Boss" }),
    };

    public override LevelRange LootLevelRange => new(1, 4);

    public override LootProbabilities LootProbabilities => new(nothing: 25f, poor: 50f, common: 25f);

    public override LootProbabilities BossLootProbabilities => new(uncommon: 100f);
}