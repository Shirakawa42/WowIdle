public class EffectDamage : Effect
{
    private readonly int damage;
    private readonly DamageType damageType;

    public EffectDamage(int damage, DamageType damageType)
    {
        this.damage = damage;
        this.damageType = damageType;
    }

    public override void Apply(Unit caster, Unit target)
    {
        target?.TakeDamage(GetDamages(caster.Stats), damageType, caster.Stats);
    }

    public override object Clone()
    {
        return new EffectDamage(damage, damageType);
    }

    public override string GetDescription()
    {
        return damage.ToString() + " " + damageType.ToString() + " damages";
    }

    public float GetDamages(Stats stats)
    {
        return CalcUtils.GetCalculatedDamage(damage, damageType, stats);
    }
}