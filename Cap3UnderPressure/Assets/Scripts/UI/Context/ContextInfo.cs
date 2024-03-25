using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ContextItem
{
    public Sprite panel;
    [Multiline] public string text;
}

[CreateAssetMenu(fileName = "ContextInfo", menuName = "Context/ContextInfo", order = 1)]
public class ContextInfo : ScriptableObject
{
    public ContextItem[] contextItems;
}
