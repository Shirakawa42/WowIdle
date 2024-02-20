using UnityEngine;

public class ColorUtils
{
    public static string GetColorFromClass(UnitClasses unitClass)
    {
        //shaman color is #0070DE
        return unitClass switch
        {
            UnitClasses.Shaman => "#0070DE",
            UnitClasses.Rogue => "#FFF569",
            UnitClasses.Mage => "#69CCF0",
            _ => "#FFFFFF",
        };
    }

    public static string GetColorFromRarity(Rarities rarity)
    {
        return rarity switch
        {
            Rarities.Poor => "#889D9D",
            Rarities.Common => "#FFFFFF",
            Rarities.Uncommon => "#1EFF0C",
            Rarities.Rare => "#0070FF",
            Rarities.Epic => "#A335EE",
            Rarities.Legendary => "#FF8000",
            _ => "#FFFFFF",
        };
    }

    public static string GetColorFromValueType(ValueType type, Rarities rarity = Rarities.Common)
    {
        return type switch
        {
            ValueType.MainStat => "#FF9300",
            ValueType.Armor => "#FFFFFF",
            ValueType.SecondaryStat => "#1EFF0C",
            ValueType.SecondaryStatPercent => "#1EFF0C",
            ValueType.Name => GetColorFromRarity(rarity),
            ValueType.Description => "#FFFFFF",
            ValueType.HP => "#FF4845",
            ValueType.Mana => "#23EAFF",
            ValueType.Level => "#EAAAFF",
            ValueType.Stamina => "#FFD100",
            ValueType.Weapon => "#FF0000",
            _ => "#FFFFFF",
        };
    }

    public static string GetTooltipColorFromValueType(ValueType type, Rarities rarity = Rarities.Common)
    {
        return type switch
        {
            ValueType.MainStat => "#FFFFFF",
            _ => GetColorFromValueType(type, rarity)
        };
    }

    public static Color GetColorFromHex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }
}
