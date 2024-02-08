using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public int strength;
    public int agility;
    public int intelligence;
    public int stamina;
    public int armor;

    public Stats(int strength, int agility, int intelligence, int stamina, int armor)
    {
        this.strength = strength;
        this.agility = agility;
        this.intelligence = intelligence;
        this.stamina = stamina;
        this.armor = armor;
    }

    public Dictionary<string, int> GetAllStats()
    {
        return new Dictionary<string, int>
        {
            { "Strength", strength },
            { "Agility", agility },
            { "Intelligence", intelligence },
            { "Stamina", stamina },
            { "Armor", armor }
        };
    }

    public Color GetStatColor(string statName)
    {
        //TODO
        return statName switch
        {
            "Strength" => Color.red,
            "Agility" => Color.green,
            "Intelligence" => Color.blue,
            "Stamina" => Color.yellow,
            _ => Color.white,
        };
    }

}
