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
            UnitClasses.Enemy
        ));
        AddHeroToInventory();
    }

    public void AddItemToInventory()
    {
        Equipment equipment = ItemUtils.GenerateEquipment(Random.Range(120, 120));
        Globals.inventoryManager.AddItem(equipment);
    }

    public void AddHeroToInventory()
    {
        Globals.heroesInventoryManager.AddHero(new Unit(
            "MacLovin",
            UnitClasses.Shaman
        ));
        Globals.heroesInventoryManager.AddHero(new Unit(
            "JeanPascal",
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
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(Globals.selectedHero.gears.GetMainHandWeapon()?.equipmentName ?? "No main hand weapon equipped");
            Debug.Log(Globals.selectedHero.gears.GetOffHandWeapon()?.equipmentName ?? "No off hand weapon equipped");
        }
        tickCooldown -= Time.deltaTime;
        if (tickCooldown <= 0)
        {
            Tick();
            tickCooldown += Globals.tickRate;
        }
    }
}
