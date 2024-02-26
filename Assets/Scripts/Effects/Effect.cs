using System;

public abstract class Effect : ICloneable
{
    public abstract void Apply(Unit caster, Unit target);
    public abstract object Clone();
    public abstract string GetDescription();
}