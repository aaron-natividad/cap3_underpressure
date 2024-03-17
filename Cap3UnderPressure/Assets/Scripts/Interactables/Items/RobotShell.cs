using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotShell : Item
{
    public RobotColor color;
    private Renderer rend;

    protected override void Initialize()
    {
        base.Initialize();
        rend = GetComponent<Renderer>();
        rend.material = new Material(rend.material);
    }

    public void ChangeColor(RobotColor rc)
    {
        color = rc;
        rend.material.color = Constants.robotColors[(int)rc];
    }
}
