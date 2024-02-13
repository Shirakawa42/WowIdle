using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public static class Globals
{
    public const float tickRate = 1f/15f;

    public static Unit selectedHero = null;
    public static Slot selectedSlot = null;
    public static List<Unit> activeHeroes = new();
    public static List<Unit> activeEnemies = new();
    public static GameManager gameManager = null;
    public static InventoryManager inventoryManager = null;
    public static HeroesInventoryManager heroesInventoryManager = null;
    public static ItemTooltipManager itemTooltipManager = null;
    public static StatsPanelManager statsPanelManager = null;
    public static EnemySlotsManager enemySlotsManager = null;
    public static GearSlotsManager gearSlotsManager = null;
}
