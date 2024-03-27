using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTaker : MonoBehaviour
{
    [SerializeField] private RecoveryStationGroup rg;
    [SerializeField] Item[] items;

    private void Start()
    {
        foreach (Item item in items)
        {
            rg.TakeFallenItem(item);
        }
    }
}
