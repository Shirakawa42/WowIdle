using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Canvas gameCanvas;
    

    void Awake() {
        Globals.gameManager = this;
        Globals.gameCanvas = gameCanvas;
    }

    void Start()
    {
        AddHeroToInventory();
    }

    public void AddItemToInventory()
    {
        Equipment equipment = ItemUtils.GenerateEquipmentOPTOREMOVE(Random.Range(1, 10));
        Globals.inventoryManager.AddItem(equipment);
    }

    public void AddHeroToInventory()
    {
        Hero shirah = new Hero(
            "Shirah",
            UnitClasses.Shaman
        );
        Globals.heroesInventoryManager.AddHero(shirah);
        Globals.heroesInventoryManager.AddHero(new Hero(
            "Jojo",
            UnitClasses.Shaman
        ));
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddItemToInventory();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Globals.dungeonManager.StartDungeon(new GreenForest());
        }
    }
}
