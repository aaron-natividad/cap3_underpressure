using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Machine
{
    [SerializeField] private AudioClip chargeSound;
    [Space(10)]
    [SerializeField] private ParticleSystem chargeEffect;
    [SerializeField] private Transform attachPoint;
    [SerializeField] private float chargeRateSeconds;

    private void Update()
    {
        if (heldItem == null) return;
        heldItem.GetComponent<RobotBattery>().Charge((100f / chargeRateSeconds) * Time.deltaTime);
    }

    public override void Interact(Player player)
    {
        if (state != MachineState.Normal) return;
        if (TryDisable()) return;

        if (player.heldItem != null)
        {
            if (!player.heldItem.GetComponent<RobotBattery>()) return;
            TakeItem(player, attachPoint);
            PlayChargeSound(true);
            OnMachineInteracted?.Invoke(this);
        }
        else if (heldItem != null)
        {
            GiveItem(player, ref heldItem);
            PlayChargeSound(false);
            OnMachineInteracted?.Invoke(this);
        }
    }

    private void PlayChargeSound(bool isPlaying)
    {
        
        audioSource.clip = chargeSound;
        audioSource.loop = isPlaying;
        if (isPlaying)
        {
            audioSource.Play();
            chargeEffect.Play();
        }
        else
        {
            audioSource.Stop();
            chargeEffect.Stop();
        }
    }
}
