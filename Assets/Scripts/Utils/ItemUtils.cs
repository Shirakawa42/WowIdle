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

    public static WeaponType GenerateRandomWeaponType()
    {
        return (WeaponType)Random.Range(0, 4);
    }

    public static Equipment GenerateEquipmentOPTOREMOVE(int itemLevel)
    {
        SlotType slot = GenerateRandomSlotType();
        Rarities rarity = GenerateRandomRarity();
        GearMat gearMat = GetRandomGearMatFromSlot(slot);

        if (slot == SlotType.OneHand || slot == SlotType.TwoHands)
        {
            float weaponSpeed = (float)Math.Round(Random.Range(0.5f, 2f), 2);
            WeaponType weaponType = GenerateRandomWeaponType();
            float damages = Mathf.Round(itemLevel * weaponSpeed * Mathf.Pow(1.005f, itemLevel) * StatsRatioBySlot(slot));
            if (weaponType == WeaponType.Shield)
                damages *= .1f;
            return new Weapon(
                GenerateEquipmentName(slot, itemLevel),
                itemLevel,
                slot,
                GenerateIcon(slot),
                rarity,
                GenerateStatsForEquipment(itemLevel, slot, rarity, weaponType == WeaponType.Shield ? GearMat.Shield : GearMat.None, weaponType),
                damages,
                weaponSpeed,
                weaponType
            );
        }
        return new(
            GenerateEquipmentName(slot, itemLevel),
            itemLevel,
            slot,
            GenerateIcon(slot),
            rarity,
            GenerateStatsForEquipment(itemLevel, slot, rarity, gearMat),
            gearMat
        );
    }

    public static Rarities GetRarityFromLootProbabilities(LootProbabilities lootProbabilities)
    {
        float random = Random.Range(0f, 100f);
        if (random <= lootProbabilities.nothing)
            return Rarities.Nothing;
        if (random <= lootProbabilities.poor + lootProbabilities.nothing)
            return Rarities.Poor;
        if (random <= lootProbabilities.common + lootProbabilities.poor + lootProbabilities.nothing)
            return Rarities.Common;
        if (random <= lootProbabilities.uncommon + lootProbabilities.common + lootProbabilities.poor + lootProbabilities.nothing)
            return Rarities.Uncommon;
        if (random <= lootProbabilities.rare + lootProbabilities.uncommon + lootProbabilities.common + lootProbabilities.poor + lootProbabilities.nothing)
            return Rarities.Rare;
        if (random <= lootProbabilities.epic + lootProbabilities.rare + lootProbabilities.uncommon + lootProbabilities.common + lootProbabilities.poor + lootProbabilities.nothing)
            return Rarities.Epic;
        if (random <= lootProbabilities.legendary + lootProbabilities.epic + lootProbabilities.rare + lootProbabilities.uncommon + lootProbabilities.common + lootProbabilities.poor + lootProbabilities.nothing)
            return Rarities.Legendary;
        return Rarities.Nothing;
    }

    public static Equipment GenerateEquipment(LevelRange levelRange, LootProbabilities lootProbabilities)
    {
        SlotType slot = GenerateRandomSlotType();
        Rarities rarity = GetRarityFromLootProbabilities(lootProbabilities);
        GearMat gearMat = GetRandomGearMatFromSlot(slot);
        int itemLevel = Random.Range(levelRange.min, levelRange.max + 1);

        if (rarity == Rarities.Nothing)
            return null;

        if (slot == SlotType.OneHand || slot == SlotType.TwoHands)
        {
            float weaponSpeed = (float)Math.Round(Random.Range(0.5f, 2f), 2);
            WeaponType weaponType = GenerateRandomWeaponType();
            float damages = Mathf.Round((3f + itemLevel) * weaponSpeed * Mathf.Pow(1.005f, itemLevel) * StatsRatioBySlot(slot));
            if (weaponType == WeaponType.Shield)
                damages *= .1f;
            return new Weapon(
                GenerateEquipmentName(slot, itemLevel),
                itemLevel,
                slot,
                GenerateIcon(slot),
                rarity,
                GenerateStatsForEquipment(itemLevel, slot, rarity, weaponType == WeaponType.Shield ? GearMat.Shield : GearMat.None, weaponType),
                damages,
                weaponSpeed,
                weaponType
            );
        }
        return new(
            GenerateEquipmentName(slot, itemLevel),
            itemLevel,
            slot,
            GenerateIcon(slot),
            rarity,
            GenerateStatsForEquipment(itemLevel, slot, rarity, gearMat),
            gearMat
        );
    }

    public static GearMat GetRandomGearMatFromSlot(SlotType slot)
    {
        return slot switch
        {
            SlotType.Cloak => GearMat.None,
            SlotType.OneHand => GearMat.None,
            SlotType.TwoHands => GearMat.None,
            SlotType.Ring => GearMat.None,
            SlotType.Neck => GearMat.None,
            SlotType.Trinket => GearMat.None,
            _ => (GearMat)Random.Range(0, 4),
        };
    }

    private static float ArmorRatioByGearMat(GearMat mat)
    {
        return mat switch
        {
            GearMat.None => 0f,
            GearMat.Cloth => .5f,
            GearMat.Leather => .86f,
            GearMat.Mail => 1.26f,
            GearMat.Plate => 1.9f,
            GearMat.Shield => 3f,
            _ => 0f,
        };
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

    private static StatIds[] PossibleStatsByWeaponType(WeaponType type)
    {
        return type switch
        {
            WeaponType.Sword => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances },
            WeaponType.Axe => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances },
            WeaponType.Mace => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances },
            WeaponType.Shield => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances },
            _ => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina },
        };
    }

    private static StatIds[] PossiblesStatsBySlot(SlotType slot, WeaponType weaponType)
    {
        return slot switch
        {
            SlotType.Head => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances },
            SlotType.Chest => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances },
            SlotType.Legs => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances },
            SlotType.Shoulders => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances },
            SlotType.Boots => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances },
            SlotType.Gloves => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances, StatIds.ParryChances, StatIds.BlockChances },
            SlotType.Belt => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances },
            SlotType.Bracers => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances },
            SlotType.Cloak => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances, StatIds.DodgeChances },
            SlotType.OneHand => PossibleStatsByWeaponType(weaponType),
            SlotType.TwoHands => PossibleStatsByWeaponType(weaponType),
            SlotType.Ring => new StatIds[] { StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances },
            SlotType.Neck => new StatIds[] { StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances },
            SlotType.Trinket => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina, StatIds.CritChances, StatIds.CritDamage, StatIds.HitChances },
            _ => new StatIds[] { StatIds.Strength, StatIds.Agility, StatIds.Intelligence, StatIds.Stamina },
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
            StatIds.CritDamage => 4f + (baseMultiplier / 40f),
            StatIds.HitChances => 1f + (baseMultiplier / 100f),
            StatIds.DodgeChances => 1f + (baseMultiplier / 100f),
            StatIds.ParryChances => 1f + (baseMultiplier / 25f),
            StatIds.BlockChances => 1f + (baseMultiplier / 15f),
            _ => baseMultiplier,
        };
    }

    public static Stats GenerateStatsForEquipment(int level, SlotType slot, Rarities rarity, GearMat mat, WeaponType weaponType = WeaponType.None)
    {
        StatIds[] possibleStats = PossiblesStatsBySlot(slot, weaponType);
        float ratio = StatsRatioBySlot(slot);
        float armorRatio = ArmorRatioByGearMat(mat);
        float ilvlRatio = level * Mathf.Pow(1.005f, level);

        Stats stats = new();

        stats[StatIds.Armor] += new Stat((float)Mathf.Round(ratio * armorRatio * ilvlRatio), StatIds.Armor);
        for (int i = 0; i < StatsLoopCountByRarity(rarity); i++)
        {
            StatIds randomStat = possibleStats[Random.Range(0, possibleStats.Length)];
            float baseMultiplier = ratio * ilvlRatio;
            stats[randomStat] += new Stat((float)Mathf.Round(CalculateRatioByStat(randomStat, baseMultiplier)), randomStat);
        }
        return stats;
    }

}
