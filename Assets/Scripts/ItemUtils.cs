using System.Net.NetworkInformation;
using UnityEngine;

public static class ItemUtils
{

    public static Color GetRarityColor(Rarities rarity)
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

    public static string GenerateEquipmentName(SlotType slot, int itemLevel, Rarities rarity)
    {
        //TODO
        string slotName = slot switch
        {
            SlotType.Head => "Helmet",
            SlotType.Chest => "Chestplate",
            SlotType.Legs => "Leggings",
            SlotType.Boots => "Boots",
            SlotType.MainHand => "Sword",
            SlotType.OffHand => "Shield",
            SlotType.Ring => "Ring",
            SlotType.Neck => "Necklace",
            SlotType.Belt => "Belt",
            SlotType.Gloves => "Gloves",
            SlotType.Shoulders => "Shoulders",
            SlotType.Bracers => "Bracer",
            _ => "Item",
        };

        string rarityName = rarity switch
        {
            Rarities.Common => "Common",
            Rarities.Uncommon => "Uncommon",
            Rarities.Rare => "Rare",
            Rarities.Epic => "Epic",
            Rarities.Legendary => "Legendary",
            _ => "Item",
        };

        return $"{rarityName} {slotName} (Lvl {itemLevel})";
    }

    public static Sprite GenerateIcon(SlotType slot, Rarities rarity)
    {
        //TODO
        return Globals.gameManager.icon;
    }

    public static SlotType GenerateRandomSlotType()
    {
        return (SlotType)Random.Range(0, 14);
    }

    public static Rarities GenerateRandomRarity()
    {
        return (Rarities)Random.Range(0, 5);
    }

    public static Equipment GenerateEquipment(int itemLevel, Rarities rarity, SlotType slot)
    {
        Equipment equipment = new(
            GenerateEquipmentName(slot, itemLevel, rarity),
            itemLevel,
            slot,
            new Stats(1 * Mathf.RoundToInt(Mathf.Max(itemLevel / 2f, 1f) * Mathf.Pow(1.01f, itemLevel)), 1, 1, 0, 0),
            GenerateIcon(slot, rarity),
            rarity
        );
        return equipment;
    }

}
