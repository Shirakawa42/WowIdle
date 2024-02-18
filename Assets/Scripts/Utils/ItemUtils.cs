using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ItemUtils
{

    public static string GenerateEquipmentName(SlotType slot, int itemLevel)
    {
        string slotName = slot switch
        {
            SlotType.Head => "Helmet",
            SlotType.Chest => "Chestplate",
            SlotType.Legs => "Leggings",
            SlotType.Boots => "Boots",
            SlotType.OneHand => "Sword",
            SlotType.TwoHands => "Big Sword",
            SlotType.Ring => "Ring",
            SlotType.Neck => "Necklace",
            SlotType.Belt => "Belt",
            SlotType.Gloves => "Gloves",
            SlotType.Shoulders => "Shoulders",
            SlotType.Bracers => "Bracer",
            SlotType.Cloak => "Cloak",
            SlotType.Trinket => "Trinket",
            _ => "Item",
        };

        return $"{slotName} (Lvl {itemLevel})";
    }

    public static Sprite GenerateIcon(SlotType slot)
    {
        string type = slot.ToString();
        if (slot == SlotType.OneHand || slot == SlotType.TwoHands)
            type = "Weapon";
        return Resources.Load<Sprite>("Textures/Equipments/" + type + "/" + Random.Range(0, 10).ToString());
    }

    public static SlotType GenerateRandomSlotType()
    {
        return (SlotType)Random.Range(0, 14);
    }

    public static Rarities GenerateRandomRarity()
    {
        return (Rarities)Random.Range(0, 6);
    }

    public static Equipment GenerateEquipment(int itemLevel)
    {
        SlotType slot = GenerateRandomSlotType();
        Rarities rarity = GenerateRandomRarity();

        if (slot == SlotType.OneHand || slot == SlotType.TwoHands)
        {
            float weaponSpeed = (float)Math.Round(Random.Range(0.5f, 2f), 2);
            return new Weapon(
                GenerateEquipmentName(slot, itemLevel),
                itemLevel,
                slot,
                GenerateIcon(slot),
                rarity,
                GenerateStatsForEquipment(itemLevel, slot, rarity),
                Mathf.Round(itemLevel * weaponSpeed * 2f),
                weaponSpeed
            );
        }
        return new(
            GenerateEquipmentName(slot, itemLevel),
            itemLevel,
            slot,
            GenerateIcon(slot),
            rarity,
            GenerateStatsForEquipment(itemLevel, slot, rarity)
        );
    }

    private static float StatsRatioBySlot(SlotType slot)
    {
        return slot switch
        {
            SlotType.Head => 1f,
            SlotType.Chest => 1f,
            SlotType.Legs => 1f,
            SlotType.Shoulders => 1f,
            SlotType.Boots => .75f,
            SlotType.Gloves => .75f,
            SlotType.Belt => .75f,
            SlotType.Bracers => .5625f,
            SlotType.Cloak => .5625f,
            SlotType.OneHand => .75f,
            SlotType.TwoHands => 1f,
            SlotType.Ring => .5625f,
            SlotType.Neck => .75f,
            SlotType.Trinket => .5625f,
            _ => .5625f,
        };
    }

    private static int StatsLoopCountByRarity(Rarities rarity)
    {
        return rarity switch
        {
            Rarities.Common => 2,
            Rarities.Uncommon => 4,
            Rarities.Rare => 7,
            Rarities.Epic => 11,
            Rarities.Legendary => 16,
            _ => 1,
        };
    }

    private static StatIds[] PossiblesStatsBySlot(SlotType slot)
    {
        return slot switch
        {
            SlotType.Head => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances, StatIds.Armor },
            SlotType.Chest => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances, StatIds.Armor },
            SlotType.Legs => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances, StatIds.Armor },
            SlotType.Shoulders => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances, StatIds.Armor },
            SlotType.Boots => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances, StatIds.Armor },
            SlotType.Gloves => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances, StatIds.Armor },
            SlotType.Belt => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances, StatIds.Armor },
            SlotType.Bracers => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances, StatIds.Armor },
            SlotType.Cloak => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances },
            SlotType.OneHand => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances },
            SlotType.TwoHands => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances },
            SlotType.Ring => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances },
            SlotType.Neck => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances },
            SlotType.Trinket => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances },
            _ => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances },
        };
    }

    private static float CalculateRatioByStat(StatIds stat, float baseMultiplier)
    {
        return stat switch
        {
            StatIds.Strength => baseMultiplier,
            StatIds.Agility => baseMultiplier,
            StatIds.Intelligence => baseMultiplier,
            StatIds.Stamina => baseMultiplier * 1.5f,
            StatIds.Armor => baseMultiplier * 2f,
            StatIds.CritChances => 1f + (baseMultiplier / 100f),
            StatIds.CritDamage => 4f + (baseMultiplier / 25f),
            StatIds.HitChances => 1f + (baseMultiplier / 100f),
            StatIds.DodgeChances => 1f + (baseMultiplier / 100f),
            StatIds.ParryChances => 1f + (baseMultiplier / 100f),
            StatIds.BlockChances => 1f + (baseMultiplier / 100f),
            _ => baseMultiplier,
        };
    }

    public static Stats GenerateStatsForEquipment(int level, SlotType slot, Rarities rarity)
    {
        StatIds[] possibleStats = PossiblesStatsBySlot(slot);
        float ratio = StatsRatioBySlot(slot);

        Stats stats = new();
        for (int i = 0; i < StatsLoopCountByRarity(rarity); i++)
        {
            StatIds randomStat = possibleStats[Random.Range(0, possibleStats.Length)];
            float baseMultiplier = ratio * (level * Mathf.Pow(1.015f, level));
            stats[randomStat] += new Stat((float)Mathf.Round(CalculateRatioByStat(randomStat, baseMultiplier)), randomStat);
        }
        return stats;
    }

}
