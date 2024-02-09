using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Sprite icon;
    private float tickCooldown = Globals.tickRate;

    void Awake() {
        Globals.gameManager = this;
    }

    void Start()
    {
        Globals.selectedHero = new Unit(
            "MacLovin",
            8,
            new UnitStats(),
            new Stats(5, 5, 5, 5, 0),
            (Weapon)ItemUtils.GenerateEquipment(16, Rarities.Common, SlotType.MainHand),
            (Weapon)ItemUtils.GenerateEquipment(8, Rarities.Common, SlotType.OffHand),
            icon,
            UnitClasses.Shaman
        );

        Globals.enemySlotsManager.AddEnemy(new Unit(
            "Joshua",
            8,
            new UnitStats(),
            new Stats(2, 2, 2, 2, 0),
            (Weapon)ItemUtils.GenerateEquipment(1, Rarities.Common, SlotType.MainHand),
            (Weapon)ItemUtils.GenerateEquipment(1, Rarities.Common, SlotType.OffHand),
            icon,
            UnitClasses.Enemy
        ));
        AddHeroToInventory();
    }

    public void AddItemToInventory()
    {
        Equipment equipment = ItemUtils.GenerateEquipment(Random.Range(1, 240), ItemUtils.GenerateRandomRarity(), ItemUtils.GenerateRandomSlotType());
        Globals.inventoryManager.AddItem(equipment);
    }

    public void AddHeroToInventory()
    {
        Globals.heroesInventoryManager.AddHero(Globals.selectedHero);
        Globals.heroesInventoryManager.AddHero(new Unit(
            "JeanPascal",
            2,
            new UnitStats(),
            new Stats(5, 5, 5, 5, 0),
            (Weapon)ItemUtils.GenerateEquipment(3, Rarities.Common, SlotType.MainHand),
            (Weapon)ItemUtils.GenerateEquipment(1, Rarities.Common, SlotType.OffHand),
            icon,
            UnitClasses.Shaman
        ));
    }

    private void Tick()
    {
        Debug.Log($"Tick! {Globals.activeHeroes.Count} heroes and {Globals.activeEnemies.Count} enemies.");
        foreach (Unit hero in Globals.activeHeroes)
            hero.Tick();
        foreach (Unit enemy in Globals.activeEnemies)
            enemy.Tick();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddItemToInventory();
        }
        tickCooldown -= Time.deltaTime;
        if (tickCooldown <= 0)
        {
            Tick();
            tickCooldown += Globals.tickRate;
        }
    }
}
