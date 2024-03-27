using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Machine : Interactable
{
    public static Action<Machine> OnMachineInteracted;

    public Item heldItem;
    public MachineState state;

    [Header("Particles")]
    [SerializeField] protected ParticleSystem explosion;
    [SerializeField] protected ParticleSystem sparks;

    [Header("SFX")]
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip disabledSound;

    protected MachineState storedState;
    protected float animSpeed = 1f;

    #region Listeners
    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    protected virtual void AddListeners()
    {
        Demotivation.OnAnimationSpeedChange += ChangeAnimationSpeed;
    }

    protected virtual void RemoveListeners()
    {
        Demotivation.OnAnimationSpeedChange -= ChangeAnimationSpeed;
    }
    #endregion

    protected override void Initialize()
    {
        base.Initialize();
        state = MachineState.Normal;
        storedState = MachineState.Normal;
    }

    public virtual IEnumerator PerformAction()
    {
        Debug.Log(gameObject.name + ": PerformAction() missing!");
        yield return null;
    }

    protected void ChangeAnimationSpeed(float animSpeed)
    {
        this.animSpeed = animSpeed;
    }

    #region Machine Disable Methods
    public void SetMachineDisabled(bool isDisabled)
    {
        state = isDisabled ? MachineState.Disabled : MachineState.Normal;
        storedState = isDisabled ? MachineState.Disabled : MachineState.Normal;
        if (outline != null) outline.OutlineColor = isDisabled ? Color.red : Color.white;
    }

    public bool TryDisable()
    {
        if (SymptomsHandler.instance == null) return false;

        if (SymptomsHandler.instance.CheckDisableRate())
        {
            StartCoroutine(CO_DisableAnimation(true, true));
            return true;
        }
        return false;
    }

    public IEnumerator CO_DisableAnimation(bool playParticles, bool disableConnected)
    {
        SetMachineDisabled(true);
        if (disableConnected) DisableConnectedMachines();

        if (playParticles)
        {
            audioSource.PlayOneShot(disabledSound);
            explosion.Play();
            sparks.Play();
        }

        yield return new WaitForSeconds(SymptomsHandler.instance.disableTime);
        SetMachineDisabled(false);
        sparks.Stop();
    }

    protected virtual void DisableConnectedMachines()
    {

    }
    #endregion

    #region Item Methods
    public void TakeItem(Item item, Transform attachPoint)
    {
        if (heldItem != null) return;
        heldItem = item;
        heldItem.Attach(attachPoint);
    }

    public void TakeItem(Player player, Transform attachPoint)
    {
        if (heldItem != null) return;
        heldItem = player.heldItem;
        player.heldItem.Drop(player);

        heldItem.Attach(attachPoint);
    }

    public void TakeItem(Player player, Vector3 position)
    {
        if (heldItem != null) return;
        heldItem = player.heldItem;
        player.heldItem.Drop(player);

        heldItem.EnablePhysics(false);
        heldItem.transform.position = position;
    }

    public void GiveItem(Player player, ref Item item)
    {
        if (player.heldItem != null) return;
        item.Pickup(player);
        item = null;
    }

    public void ClearItem()
    {
        if (heldItem == null) return;
        Destroy(heldItem.gameObject);
        heldItem = null;
    }
    #endregion
}
