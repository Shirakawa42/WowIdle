using System.Collections.Generic;
using UnityEngine;

public class EnemySlotsManager : MonoBehaviour
{
    public GameObject enemySlots;

    void Awake()
    {
        Globals.enemySlotsManager = this;
    }

    public void AddEnemy(Unit unit)
    {
        foreach (Transform child in enemySlots.transform)
        {
            if (child.GetComponent<Slot>().IsEmpty())
            {
                child.GetComponent<Slot>().SetSlotable(unit);
                return;
            }
        }
    }
}
