using UnityEngine;

public static class CalcUtils
{
    public static float CalculateWeaponDamage(Weapon weapon, Stats stats, bool mainHand)
    {
        if (weapon == null)
            return 0;

        float offHandMultiplier = 0.75f;
        if (weapon.SlotType == SlotType.TwoHands)
            offHandMultiplier = 0.25f;

        if (mainHand)
            return Mathf.RoundToInt(weapon.damages + stats[StatIds.Strength].value);
        else
            return Mathf.RoundToInt((weapon.damages + stats[StatIds.Strength].value) * offHandMultiplier);
    }
}