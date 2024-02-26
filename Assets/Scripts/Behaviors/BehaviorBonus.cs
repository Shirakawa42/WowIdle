using System.Collections.Generic;

public class BehaviorBonus : Behavior
{
    private readonly float duration;
    private float currentDuration = 0f;
    private readonly float period;
    private float currentPeriod = 0f;
    private readonly Effect onApplyEffect;
    private readonly Effect onPeriodEffect;
    private readonly Effect onExpireEffect;

    public BehaviorBonus(Unit caster, Unit target, bool isBuff, string name, string icon, float duration, float period, Effect onApplyEffect, Effect onPeriodEffect, Effect onExpireEffect)
            : base(caster, target, isBuff, name, icon)
    {
        this.duration = duration;
        this.period = period;
        this.onApplyEffect = onApplyEffect;
        this.onPeriodEffect = onPeriodEffect;
        this.onExpireEffect = onExpireEffect;
        onApplyEffect?.Apply(Caster, Target);
        Description = GenerateDescription();
    }

    public override bool Tick()
    {
        if (currentDuration < duration)
        {
            currentDuration += Globals.tickRate;
            if (currentPeriod < period)
                currentPeriod += Globals.tickRate;
            else if (period > 0f)
            {
                onPeriodEffect?.Apply(Caster, Target);
                currentPeriod -= period;
            }
        }
        else
        {
            onExpireEffect?.Apply(Caster, Target);
            return false;
        }
        return true;
    }

    public override object Clone()
    {
        return new BehaviorBonus(Caster, Target, IsBuff, Name, Icon, duration, period, (Effect)onApplyEffect.Clone(), (Effect)onPeriodEffect.Clone(), (Effect)onExpireEffect.Clone());
    }

    public override List<TooltipValue> GetTooltipValues()
    {
        return new() { new TooltipValue(Name, "", ValueType.Name), new TooltipValue(Description, "", ValueType.Description)};
    }

    public override string GenerateDescription()
    {
        string description = "";
        if (onApplyEffect != null)
            description += "On apply: " + onApplyEffect.GetDescription();
        if (onPeriodEffect != null)
            description += "\nEvery " + period + " seconds: " + onPeriodEffect.GetDescription();
        if (onExpireEffect != null)
            description += "\nOn expire: " + onExpireEffect.GetDescription();
        return description;
    }
}
