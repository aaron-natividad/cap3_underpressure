using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public AudioSource audioSource;

    [Header("Inventory")]
    public Transform hand;
    [HideInInspector] public Item heldItem;

    [Header("Components")]
    public PlayerController controller;
    public PlayerMovement movement;
    public PlayerInteract interaction;
    public PlayerCamera fpsCam;
    [HideInInspector] public Rigidbody rigidBody;
    [HideInInspector] public Collider collision;

    // States
    [HideInInspector] public bool grounded;
    [HideInInspector] public PlayerState state = PlayerState.Normal;

    // Status Flags
    public bool heldItemRunEnabled = true;

    private void Awake()
    {
        instance = this;
        rigidBody = GetComponent<Rigidbody>();
        collision = GetComponent<Collider>();
    }

    public void ChangeState(PlayerState state)
    {
        this.state = state;
    }
}
