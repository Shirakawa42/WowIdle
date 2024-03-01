using System.Collections.Generic;

public class AbilityEffectTarget : Ability
{
    private readonly Effect effect;

    public AbilityEffectTarget(Unit owner, string name, string icon, float cooldown, float manaCost, string description, Effect effect) : base(owner, name, icon, cooldown, manaCost, description)
    {
        this.effect = effect;
    }

    public override void Cast()
    {
        if (CurrentCooldown <= 0 && Owner.Stats[StatIds.CurrentMana].value >= ManaCost && Owner.Target != null)
        {
            Color = "#FF0000";
            effect.Apply(Owner, Owner.Target);
            Owner.Stats[StatIds.CurrentMana].value -= ManaCost;
            CurrentCooldown += Cooldown;
        }
    }

    public override object Clone()
    {
        return new AbilityEffectTarget(Owner, Name, Icon, Cooldown, ManaCost, Description, (Effect)effect.Clone());
    }

    public override List<TooltipValue> GetTooltipValues()
    {
        string description = Description.Replace("$D", (effect as EffectDamage).GetDamages(Owner.Stats).ToString());

        return new List<TooltipValue>
        {
            new(Name, "", ValueType.Name),
            new(description, "", ValueType.Description),
            new("Cooldown", Cooldown.ToString(), ValueType.SecondaryStat),
            new("Mana Cost", ManaCost.ToString(), ValueType.Mana)
        };
    }
}