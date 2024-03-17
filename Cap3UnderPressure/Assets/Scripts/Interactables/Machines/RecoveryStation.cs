using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryStation : Machine
{
    [SerializeField] private Transform attachPoint;

    private RecoveryStationGroup group;

    protected override void Initialize()
    {
        base.Initialize();
    }

    public override void Interact(Player player)
    {
        if (state != MachineState.Normal) return;

        if (heldItem != null && player.heldItem == null)
        {
            GiveItem(player, ref heldItem);
            group.availableList.Add(this);
            group.occupiedList.Remove(this);
            OnMachineInteracted?.Invoke(this);
        }
    }

    public void TakePlayerItem()
    {
        TakeItem(Player.instance, attachPoint);
        group.availableList.Remove(this);
        group.occupiedList.Add(this);
    }

    public void TakeFallenItem(Item item)
    {
        TakeItem(item, attachPoint);
        group.availableList.Remove(this);
        group.occupiedList.Add(this);
    }

    public void StoreGroup(RecoveryStationGroup group)
    {
        this.group = group;
        group.availableList.Add(this);
    }
}
