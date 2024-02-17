using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float tickCooldown = Globals.tickRate;

    void Awake() {
        Globals.gameManager = this;
    }

    void Start()
    {
        Globals.enemySlotsManager.AddEnemy(new Unit(
            "Joshua",
            8,
            new UnitStats(),
            new Stats(2, 2, 2, 2, 0),
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
        Globals.heroesInventoryManager.AddHero(new Unit(
            "MacLovin",
            8,
            new UnitStats(),
            new Stats(5, 5, 5, 5, 0),
            UnitClasses.Shaman
        ));
        Globals.heroesInventoryManager.AddHero(new Unit(
            "JeanPascal",
            2,
            new UnitStats(),
            new Stats(5, 5, 5, 5, 0),
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
