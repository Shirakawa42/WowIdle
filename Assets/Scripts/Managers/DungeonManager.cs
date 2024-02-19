using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public GameObject enemiesSlots;
    public GameObject heroesSlots;

    public List<Unit> activeHeroes = new();
    public List<Unit> activeEnemies = new();

    private float tickCooldown = Globals.tickRate;

    private Dungeon currentDungeon;
    private int currentFloor = 0;
    private int maxFloor;
    private Queue<Enemy> enemiesToSpawn = new();
    private float floorCooldown = Globals.timeBetweenFloors;

    void Awake()
    {
        Globals.dungeonManager = this;
    }

    private void Tick()
    {
        foreach (Unit hero in activeHeroes)
            hero.Tick();
        foreach (Unit enemy in activeEnemies)
            enemy.Tick();

        if (activeEnemies.Count == 0 && currentFloor < maxFloor)
            floorCooldown -= Globals.tickRate;
        if (floorCooldown <= 0)
        {
            NextFloor();
            floorCooldown = Globals.timeBetweenFloors;
        }
    }

    public void StartDungeon(Dungeon dungeon)
    {
        currentDungeon = dungeon;
        maxFloor = dungeon.Floors.Count;
        currentFloor = 0;
        NextFloor();
        Debug.Log("Starting dungeon: " + dungeon.Name);
    }

    private void NextFloor()
    {
        foreach (string enemy in currentDungeon.Floors[currentFloor].enemies)
            AddEnemy(currentDungeon.EnemyTypes[enemy].Clone() as Enemy);
        currentFloor++;
    }

    public void AddEnemy(Enemy unit)
    {
        foreach (Transform child in enemiesSlots.transform)
        {
            if (child.GetComponent<Slot>().IsEmpty())
            {
                child.GetComponent<Slot>().SetSlotable(unit);
                return;
            }
        }
        enemiesToSpawn.Enqueue(unit);
    }

    public void OnEnemyDeath(Enemy enemy)
    {
        activeEnemies.Remove(enemy);
        foreach (Unit hero in activeHeroes)
            hero.AddXP(enemy.droppedExp);
        enemy.Remove();
        if (enemiesToSpawn.Count > 0)
            AddEnemy(enemiesToSpawn.Dequeue());
        
    }

    void Update()
    {
        tickCooldown -= Time.deltaTime;
        if (tickCooldown <= 0)
        {
            Tick();
            tickCooldown += Globals.tickRate;
        }
    }
}