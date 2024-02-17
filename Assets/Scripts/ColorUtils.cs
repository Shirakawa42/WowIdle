using UnityEngine;

public class ColorUtils
{
    public static Color GetColorFromClass(UnitClasses unitClass)
    {
        return unitClass switch
        {
            UnitClasses.Enemy => Color.red,
            UnitClasses.Shaman => Color.blue,
            UnitClasses.Rogue => Color.yellow,
            UnitClasses.Mage => Color.cyan,
            _ => Color.white,
        };
    }

    public static Color GetColorFromRarity(Rarities rarity)
    {
        return rarity switch
        {
            Rarities.Common => Color.white,
            Rarities.Uncommon => Color.green,
            Rarities.Rare => Color.blue,
            Rarities.Epic => Color.magenta,
            Rarities.Legendary => Color.yellow,
            _ => Color.white,
        };
    }

    public static Color GetColorFromTooltipValueType(TooltipValueType type, Rarities rarity = Rarities.Common)
    {
        return type switch
        {
            TooltipValueType.MainStat => Color.white,
            TooltipValueType.SecondaryStat => Color.green,
            TooltipValueType.Name => GetColorFromRarity(rarity),
            TooltipValueType.Description => Color.yellow,
            _ => Color.white,
        };
    }
}
