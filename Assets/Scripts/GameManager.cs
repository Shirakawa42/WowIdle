using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Sprite icon;

    void Awake() {
        Globals.gameManager = this;
    }

    void Start()
    {
        
    }

    public void AddItemToInventory()
    {
        Equipment equipment = ItemUtils.GenerateEquipment(Random.Range(1, 240), ItemUtils.GenerateRandomRarity(), ItemUtils.GenerateRandomSlotType());
        Globals.inventoryManager.AddItem(equipment);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddItemToInventory();
        }
    }
}
