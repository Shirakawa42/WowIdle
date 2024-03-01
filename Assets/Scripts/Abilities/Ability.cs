using System;
using UnityEngine;

public abstract class Ability : Hoverable, ICloneable
{
    public Unit Owner { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public float Cooldown { get; set; }
    public float CurrentCooldown { get; set; }
    public float ManaCost { get; set; }

    public override string Color { get; set; }
    public override Vector3 TooltipPosition { get; set; }

    public Ability(Unit owner, string name, string icon, float cooldown, float manaCost, string description, string color = "#00FF00")
    {
        Owner = owner;
        Name = name;
        Icon = icon;
        Color = color;
        Description = description;
        Cooldown = cooldown;
        CurrentCooldown = 0f;
        ManaCost = manaCost;
    }

    public abstract void Cast();

    public void Tick()
    {
        if (CurrentCooldown > 0)
        {
            CurrentCooldown -= Globals.tickRate;
        }
        else
        {
            Color = "#00FF00";
            Cast();
        }
    }

    public void SetTooltipPosition(Vector3 position)
    {
        TooltipPosition = position;
    }

    public abstract object Clone();
}