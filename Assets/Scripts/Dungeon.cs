using System.Collections.Generic;

public class Floor
{
    public List<string> enemies;

    public Floor(List<string> enemies)
    {
        this.enemies = enemies;
    }
}

public abstract class Dungeon
{
    public abstract List<Floor> Floors { get; }
    public abstract string Name { get; }
    public abstract Dictionary<string, Enemy> EnemyTypes { get; }
}