using UnityEngine;

public static class CalcUtils
{
    public static float CalculateWeaponDamage(Weapon weapon, Unit unit, bool mainHand)
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
            return Mathf.RoundToInt((weapon.damages + weapon.damages * (unit.stats[StatIds.Strength].value / 10)) * damageMultiplier);
        else
            return Mathf.RoundToInt((weapon.damages + weapon.damages * (unit.stats[StatIds.Strength].value / 10)) * offHandMultiplier * damageMultiplier);
    }
}