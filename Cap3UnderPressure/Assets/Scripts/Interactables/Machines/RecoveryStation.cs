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

    protected override void OnStart()
    {
        StartCoroutine(CO_DelayedAttach());
    }

    private IEnumerator CO_DelayedAttach()
    {
        yield return new WaitForSeconds(0.2f);
        TakeItem(heldItem, attachPoint);
        group.availableList.Remove(this);
        group.occupiedList.Add(this);
        arrow.SetActive(heldItem != null);
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
