using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPart : Item
{
    public RobotColor roombaColor;
    public RobotPartType type;
    
    private Renderer rend;
    private Color redColor = new Color(0.5f, 0.2f, 0.2f, 1f);
    private Color blueColor = new Color(0.2f, 0.2f, 0.5f, 1f);
    private Color greenColor = new Color(0.2f, 0.5f, 0.2f, 1f);

    protected override void Initialize()
    {
        base.Initialize();
        rend = GetComponent<Renderer>();
        rend.material = new Material(rend.material);
    }

    public void ChangeColor(RobotColor rc)
    {
        roombaColor = rc;
        switch (rc)
        {
            case RobotColor.Red:
                rend.material.color = redColor;
                break;
            case RobotColor.Blue:
                rend.material.color = blueColor;
                break;
            case RobotColor.Green:
                rend.material.color = greenColor;
                break;
        }
    }
}
