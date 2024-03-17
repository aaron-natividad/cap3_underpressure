using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraulicPress : Machine
{
    [SerializeField] private AudioClip shutterSound;
    [SerializeField] private AudioClip pressSound;

    [Header("Press Particles")]
    [SerializeField] private ParticleSystem pressParticle;
    
    [Header("Press Objects")]
    [SerializeField] private GameObject resultPrefab;
    [SerializeField] private GameObject pressObject;
    [SerializeField] private GameObject shutter;
    [SerializeField] private Transform attachPoint;

    [Header("Press Parameters")]
    [SerializeField] private float pressDistance;
    [SerializeField] private float shutterDistance;

    private Vector3 pressLocation;
    private Vector3 shutterLocation;

    protected override void Initialize()
    {
        base.Initialize();
        pressLocation = pressObject.transform.position;
        shutterLocation = shutter.transform.position;
    }

    public override void Interact(Player player)
    {
        if (state != MachineState.Normal) return;

        if (player.heldItem != null)
        {
            TakeItem(player, attachPoint);
            OnMachineInteracted?.Invoke(this);
        }
        else if (heldItem != null)
        {
            GiveItem(player, ref heldItem);
            OnMachineInteracted?.Invoke(this);
        }
    }

    public override IEnumerator PerformAction()
    {
        state = MachineState.Performing;
        audioSource.PlayOneShot(shutterSound);
        LeanTween.move(shutter, shutterLocation - new Vector3(0, shutterDistance, 0), 0.05f * animSpeed);
        yield return new WaitForSeconds(0.5f * animSpeed);
        LeanTween.move(pressObject, pressLocation - new Vector3(0, pressDistance, 0), 0.05f * animSpeed);
        yield return new WaitForSeconds(0.05f * animSpeed);
        pressParticle.Play();
        audioSource.PlayOneShot(pressSound);
        yield return new WaitForSeconds(0.05f * animSpeed);

        EvaluateItem();
        yield return new WaitForSeconds(0.5f * animSpeed);

        LeanTween.move(shutter, shutterLocation, 0.9f * animSpeed).setEase(LeanTweenType.easeOutCubic);
        LeanTween.move(pressObject, pressLocation, 0.9f * animSpeed).setEase(LeanTweenType.easeOutCubic);
        yield return new WaitForSeconds(0.9f * animSpeed);
        state = MachineState.Normal;
    }

    private void EvaluateItem()
    {
        Item spawnedItem;
        string itemName;

        if (heldItem == null) return;
        itemName = heldItem.id;
        ClearItem();

        if (itemName == "scrap")
        {
            spawnedItem = Instantiate(resultPrefab, attachPoint).GetComponent<Item>();
            heldItem = spawnedItem;
            heldItem.Attach(attachPoint);
        }
    }
}
