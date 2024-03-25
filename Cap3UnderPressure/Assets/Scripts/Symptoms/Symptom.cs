using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Symptom : ScriptableObject
{
    public Sprite icon;
    public VideoClip clip;
    public string id;
    [TextArea] public string description;
    [TextArea] public string effect;

    [HideInInspector] public SymptomIcon ui;
    [HideInInspector] public SymptomsHandler handler;

    public virtual void StartBehavior(SymptomsHandler handler)
    {
        ui.UpdateIcon(icon);
    }

    public virtual void EndBehavior(SymptomsHandler handler)
    {

    }
}
