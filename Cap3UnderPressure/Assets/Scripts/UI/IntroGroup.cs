using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntroItem
{
    public Sprite panel;
    [Multiline] public string text;
}

[CreateAssetMenu(fileName = "IntroGroup", menuName = "Intro/IntroGroup", order = 1)]
public class IntroGroup : ScriptableObject
{
    public IntroItem[] introItems;
}
