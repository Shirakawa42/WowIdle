using System;
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

    public (string, int)[] GetAllStats()
    {
        return new (string, int)[]
        {
            ( "Strength", strength ),
            ( "Agility", agility ),
            ( "Intelligence", intelligence ),
            ( "Stamina", stamina ),
            ( "Armor", armor)
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

    public static Stats operator +(Stats a, Stats b)
    {
        return new Stats(a.strength + b.strength, a.agility + b.agility, a.intelligence + b.intelligence, a.stamina + b.stamina, a.armor + b.armor);
    }

    public static Stats operator -(Stats a, Stats b)
    {
        return new Stats(a.strength - b.strength, a.agility - b.agility, a.intelligence - b.intelligence, a.stamina - b.stamina, a.armor - b.armor);
    }

}
