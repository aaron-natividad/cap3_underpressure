using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IN_Sample : Interactable
{
    public Material matOne;
    public Material matTwo;

    private bool matSwitch = false;

    public override void Interact()
    {
        GetComponent<Renderer>().material = matSwitch ? matOne : matTwo;
        matSwitch = !matSwitch;
    }
}
