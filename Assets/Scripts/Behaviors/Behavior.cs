using System;
using UnityEngine;

public abstract class Behavior : Hoverable, ICloneable
{
    public Unit Caster { get; set; }
    public Unit Target { get; set; }
    public bool IsBuff { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }

    public override string Color { get; set; }
    public override Vector3 TooltipPosition { get; set; }

    public Behavior(Unit caster, Unit target, bool isBuff, string name, string icon)
    {
        Caster = caster;
        Target = target;
        IsBuff = isBuff;
        Name = name;
        Icon = icon;
        Color = isBuff ? "#00FF00" : "#FF0000";
    }

    public abstract string GenerateDescription();

    public abstract bool Tick();

    public abstract object Clone();
    
    public void SetTooltipPosition(Vector3 position)
    {
        TooltipPosition = position;
    }
}