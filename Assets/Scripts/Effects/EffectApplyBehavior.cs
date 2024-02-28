public class EffectApplyBehavior : Effect
{
    private readonly Behavior behavior;

    public EffectApplyBehavior(Behavior behavior)
    {
        this.behavior = behavior;
    }

    public override void Apply(Unit caster, Unit target)
    {
        target.AddBehavior(behavior);
    }

    public override object Clone()
    {
        return new EffectApplyBehavior(behavior);
    }

    public override string GetDescription()
    {
        return "Apply " + behavior.Name;
    }
}