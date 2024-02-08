using System.Collections.Generic;
using UnityEngine;

public class UnitStats
{
    public int currentHealth;
    public int maxHealth;
    public int currentResource;
    public int maxResource;
    public int currentXP;
    public int maxXP;
    public int level;
    public int maxLevel = 60;

    public (string, int)[] GetAllStats()
    {
        return new (string, int)[]
        {
            ( "HP", maxHealth ),
            ( "Mana", maxResource )
        };
    }

    public Color GetStatColor(string statName)
    {
        return statName switch
        {
            "HP" => new Color(0xff, 0x00, 0x00),
            "Mana" => Color.cyan,
            "Strength" => new Color(0xff, 0x00, 0xe2),
            "Agility" => new Color(0xff, 0x00, 0xe2),
            "Intelligence" => new Color(0xff, 0x00, 0xe2),
            "Stamina" => Color.green,
            "Armor" => Color.green,
            _ => Color.white,
        };
    }

    public UnitStats()
    {
        this.maxHealth = 100;
        this.maxResource = 100;
        this.currentHealth = 100;
        this.currentResource = 100;
        this.currentXP = 0;
        this.maxXP = 100;
        this.level = 1;
    }
}
