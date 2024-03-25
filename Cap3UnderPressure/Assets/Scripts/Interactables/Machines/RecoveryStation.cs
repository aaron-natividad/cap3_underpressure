using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryStation : Machine
{
    [SerializeField] private Transform attachPoint;
    [SerializeField] private GameObject arrow;

    private RecoveryStationGroup group;

    protected override void Initialize()
    {
        base.Initialize();
        arrow.SetActive(heldItem != null);
        if (heldItem != null) heldItem.Attach(attachPoint);
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
            arrow.SetActive(false);
        }
    }

    public void TakePlayerItem()
    {
        TakeItem(Player.instance, attachPoint);
        group.availableList.Remove(this);
        group.occupiedList.Add(this);
        if (heldItem != null) arrow.SetActive(true);
    }

    public void TakeFallenItem(Item item)
    {
        TakeItem(item, attachPoint);
        group.availableList.Remove(this);
        group.occupiedList.Add(this);
        if (heldItem != null) arrow.SetActive(true);
    }

    public void StoreGroup(RecoveryStationGroup group)
    {
        this.group = group;
        group.availableList.Add(this);
    }
}
