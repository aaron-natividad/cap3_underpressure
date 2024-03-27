using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "LevelInfo", order = 1)]
public class LevelInfo : ScriptableObject
{
    public string title;
    public string queuedLevel;
    public Sprite screenshot;
}
