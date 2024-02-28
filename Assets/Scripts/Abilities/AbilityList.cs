public static class AbilityList
{
    public static readonly Ability[] Abilities = new Ability[]
    {
        new AbilityEffectTarget(
                owner: null,
                name: "Stormstrike",
                icon: "stormstrike",
                cooldown: 5f,
                manaCost: 5f,
                description: "Strike the target for 20 physical damages.",
                effect: new EffectDamage(20, DamageType.Physical)
            )
    };

    public static Ability GetAbility(AbilityIds id)
    {
        return Abilities[(int)id].Clone() as Ability;
    }
}
