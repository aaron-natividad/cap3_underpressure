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
            RobotBattery battery = player.heldItem.GetComponent<RobotBattery>();
            if (battery == null) return;
            
            battery.OnFullCharge += DisableChargeSound;
            ToggleChargeSound(!battery.IsFullyCharged());

            TakeItem(player, attachPoint);
            OnMachineInteracted?.Invoke(this);
        }
        else if (heldItem != null)
        {
            heldItem.GetComponent<RobotBattery>().OnFullCharge -= DisableChargeSound;
            ToggleChargeSound(false);
            GiveItem(player, ref heldItem);
            OnMachineInteracted?.Invoke(this);
        }
    }

    private void DisableChargeSound()
    {
        ToggleChargeSound(false);
    }

    private void ToggleChargeSound(bool isPlaying)
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
