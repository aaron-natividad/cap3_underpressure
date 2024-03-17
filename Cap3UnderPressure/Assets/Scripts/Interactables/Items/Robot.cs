using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : Item
{
    [Header("Roomba Parts")]
    public RobotShell shell;
    public RobotBattery battery;
    public RobotWheels wheels;
    [Space(10)]
    [SerializeField] private Vector3 shellPos;
    [SerializeField] private Vector3 batteryPos;
    [SerializeField] private Vector3 wheelPos;

    protected override void Initialize()
    {
        base.Initialize();
        DisableAllParts();
    }

    public bool AttachPart(Item part)
    {
        Vector3 pos;

        if (part.GetComponent<RobotShell>() && shell == null)
        {
            shell = part.GetComponent<RobotShell>();
            pos = shellPos;
        }
        else if (part.GetComponent<RobotBattery>() && battery == null)
        {
            battery = part.GetComponent<RobotBattery>();
            pos = batteryPos;
        }
        else if (part.GetComponent<RobotWheels>() && wheels == null)
        {
            wheels = part.GetComponent<RobotWheels>();
            pos = wheelPos;
        }
        else
        {
            return false;
        }

        part.transform.parent = transform;
        DisablePart(part, pos);
        outline.Reinitialize();
        return true;
    }

    private void DisablePart(Item part, Vector3 partPos)
    {
        if (part == null) return;
        part.GetComponent<Collider>().enabled = false;
        part.GetComponent<Rigidbody>().isKinematic = true;
        part.GetComponent<Outline>().enabled = false;

        part.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        part.transform.localPosition = partPos;
    }

    private void DisableAllParts()
    {
        DisablePart(shell, shellPos);
        DisablePart(battery, batteryPos);
        DisablePart(wheels, wheelPos);
    }

    public bool IsComplete()
    {
        return shell != null && battery != null && wheels != null;
    }

    public bool IsCorrect(RobotColor requiredColor = RobotColor.White)
    {
        return shell.color == requiredColor && battery.IsFullyCharged();
    }
}
