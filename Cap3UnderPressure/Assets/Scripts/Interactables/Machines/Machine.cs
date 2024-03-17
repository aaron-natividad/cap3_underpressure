using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Machine : Interactable
{
    public static Action<Machine> OnMachineInteracted;

    [Header("Particles")]
    [SerializeField] protected ParticleSystem explosion;
    [SerializeField] protected ParticleSystem sparks;

    [Header("SFX")]
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip disabledSound;

    [HideInInspector] public Item heldItem;
    [HideInInspector] public MachineState state;

    protected float animSpeed = 1f;

    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    protected override void Initialize()
    {
        base.Initialize();
        state = MachineState.Normal;
    }

    public virtual IEnumerator PerformAction()
    {
        Debug.Log(gameObject.name + ": PerformAction() missing!");
        yield return null;
    }

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

    public bool TryDisable()
    {
        if (SymptomsHandler.instance == null) return false;

        if (SymptomsHandler.instance.CheckDisableRate())
        {
            StartCoroutine(DisableMachine(true, true));
            return true;
        }
        return false;
    }

    public IEnumerator DisableMachine(bool playParticles, bool disableConnected)
    {
        state = MachineState.Disabled;
        if (outline != null) outline.OutlineColor = Color.red;
        if (disableConnected) DisableConnectedMachines();

        if (playParticles)
        {
            audioSource.PlayOneShot(disabledSound);
            explosion.Play();
            sparks.Play();
        }

        yield return new WaitForSeconds(SymptomsHandler.instance.disableTime);
        sparks.Stop();
        if (outline != null) outline.OutlineColor = Color.white;
        state = MachineState.Normal;
    }

    protected virtual void DisableConnectedMachines()
    {

    }

    protected void ChangeAnimationSpeed(float animSpeed)
    {
        this.animSpeed = animSpeed;
    }

    protected virtual void AddListeners()
    {
        Demotivation.OnAnimationSpeedChange += ChangeAnimationSpeed;
    }

    protected virtual void RemoveListeners() 
    {
        Demotivation.OnAnimationSpeedChange -= ChangeAnimationSpeed;
    }
}
