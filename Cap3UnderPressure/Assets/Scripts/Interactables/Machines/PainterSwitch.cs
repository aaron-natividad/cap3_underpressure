using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainterSwitch : ButtonBase
{
    [SerializeField] private RobotColor paintColor;

    public override void Interact(Player pc)
    {
        machine.GetComponent<Painter>().currentPaintColor = paintColor;
        base.Interact(pc);
    }

    protected override void DisableConnectedMachines()
    {
        machine.StartCoroutine(machine.DisableMachine(false, true));
    }
}
