using UnityEngine;

public static class CalcUtils
{
    public static float CalculateWeaponDamage(Weapon weapon, Hero unit, bool mainHand)
    {
        if (weapon == null)
            return 0;

        float offHandMultiplier = 0.75f;
        float damageMultiplier = 1f;

        Weapon mainHandWeapon = unit.gears.GetMainHandWeapon();
        Weapon offHandWeapon = unit.gears.GetOffHandWeapon();
        if (mainHandWeapon != null && offHandWeapon != null)
        {
            if (mainHandWeapon.SlotType == SlotType.TwoHands || offHandWeapon.SlotType == SlotType.TwoHands)
                damageMultiplier = .2f;
        }

        if (mainHand)
            return Mathf.RoundToInt((weapon.damages + weapon.damages * (unit.Stats[StatIds.Strength].value / 10)) * damageMultiplier);
        else
            return Mathf.RoundToInt((weapon.damages + weapon.damages * (unit.Stats[StatIds.Strength].value / 10)) * offHandMultiplier * damageMultiplier);
    }

    public static float GetArmorReduction(float armor, float level)
    {
        return armor / (armor + (240f*(level * Mathf.Pow(Globals.globalExponentialGrowth, level*2f)))) * 100f;
    }

    public static float GetCalculatedDamage(float damage, DamageType damageType, Stats stats)
    {
        if (damageType == DamageType.Physical)
            return damage + (stats[StatIds.Strength].value / 10f);
        else if (damageType == DamageType.Fire)
            return damage + (stats[StatIds.Intelligence].value / 10f);
        return damage;
    }
}