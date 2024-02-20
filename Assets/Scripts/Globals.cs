using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public static class Globals
{
    public const float tickRate = 1f/15f;
    public const int maxLevel = 60;
    public const float timeBetweenFloors = tickRate * 15f;

    public static Hero selectedHero = null;
    public static Slot selectedSlot = null;
    public static Canvas gameCanvas = null;
    public static GameManager gameManager = null;
    public static InventoryManager inventoryManager = null;
    public static HeroesInventoryManager heroesInventoryManager = null;
    public static ItemTooltipManager itemTooltipManager = null;
    public static StatsPanelManager statsPanelManager = null;
    public static GearSlotsManager gearSlotsManager = null;
    public static ResourcesManager resourcesManager = null;
    public static DungeonManager dungeonManager = null;
}
