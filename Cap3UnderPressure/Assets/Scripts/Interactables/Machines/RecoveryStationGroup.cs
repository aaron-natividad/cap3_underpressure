using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryStationGroup : MonoBehaviour
{
    public List<RecoveryStation> availableList;
    public List<RecoveryStation> occupiedList;

    private void Awake()
    {
        availableList = new List<RecoveryStation>();
        occupiedList = new List<RecoveryStation>();

        foreach (Transform child in transform)
        {
            RecoveryStation rs = child.GetComponent<RecoveryStation>();
            if (rs != null)
            {
                rs.StoreGroup(this);
            }
        }
    }

    public void TakePlayerItem()
    {
        if (availableList.Count <= 0) ClearOldestItem();
        int index = Random.Range(0, availableList.Count);
        if (Player.instance.heldItem != null)
        {
            availableList[index].TakePlayerItem();
        }
    }

    public void TakeFallenItem(Item item)
    {
        if (availableList.Count <= 0) ClearOldestItem();
        int index = Random.Range(0, availableList.Count);
        availableList[index].TakeFallenItem(item);
    }

    public void ClearOldestItem()
    {
        occupiedList[0].ClearItem();
        availableList.Add(occupiedList[0]);
        occupiedList.RemoveAt(0);
    }
}
