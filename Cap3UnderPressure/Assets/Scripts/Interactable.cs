using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public float interactTime;

    private Outline outline;
    
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void Highlight(bool isHighlighted)
    {
        outline.enabled = isHighlighted;
    }

    public abstract void Interact();
}
