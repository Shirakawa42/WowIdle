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
        Equipment equipment = ItemUtils.GenerateEquipment(Random.Range(1, 20));
        Globals.inventoryManager.AddItem(equipment);
    }

    public void AddHeroToInventory()
    {
        Globals.heroesInventoryManager.AddHero(new Unit(
            "Shirah",
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
