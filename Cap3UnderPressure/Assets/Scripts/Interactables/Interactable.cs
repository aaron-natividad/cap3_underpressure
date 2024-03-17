using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string id;
    protected Outline outline;
    protected Collider col;
    
    private void Awake()
    {
        col = GetComponent<Collider>();
        Initialize();
    }

    public void Highlight(bool isHighlighted)
    {
        if (outline == null) return;
        outline.enabled = isHighlighted;
    }

    public abstract void Interact(Player player);

    // overrideable awake function
    protected virtual void Initialize()
    {
        outline = GetComponent<Outline>();
        if (outline != null) outline.enabled = false;
    }
}
