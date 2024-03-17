using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Interactable
{
    public static Action<Item> OnItemPickup;

    [SerializeField] private AudioClip pickupSound;

    protected Rigidbody rigidBody;
    protected AudioSource audioSource;

    protected override void Initialize()
    {
        base.Initialize();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    public override void Interact(Player player)
    {
        Pickup(player);
    }

    public virtual void Pickup(Player player)
    {
        if (player.heldItem != null) return;

        player.heldItem = this;
        Attach(player.hand);
        audioSource.PlayOneShot(pickupSound);
        OnItemPickup?.Invoke(this);
    }

    public virtual void Attach(Transform parent)
    {
        transform.parent = parent;
        EnablePhysics(false);
        col.enabled = false;
    }

    public virtual void Drop(Player player)
    {
        EnablePhysics(true);
        col.enabled = true;
        transform.up = Vector3.up;
        transform.forward = player.transform.forward;

        player.heldItem = null;
        transform.parent = null;
    }

    public virtual void EnablePhysics(bool isEnabled)
    {
        rigidBody.isKinematic = !isEnabled;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
