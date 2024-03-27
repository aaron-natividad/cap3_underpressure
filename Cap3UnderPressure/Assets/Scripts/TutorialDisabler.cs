using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MachineGroup
{
    public int groupIndex;
    public List<Machine> enabledMachines;
}

public class TutorialDisabler : MonoBehaviour
{
    [SerializeField] private List<Machine> machinesMaster;
    [Space(10)]
    [SerializeField] private MachineGroup[] sequence;
    
    private void OnEnable()
    {
        DialogueHandler.OnDialogueGroupStart += EnableMachines;
    }

    private void OnDisable()
    {
        DialogueHandler.OnDialogueGroupStart -= EnableMachines;
    }

    private void DisableAllMachines()
    {
        foreach (Machine machine in machinesMaster)
        {
            machine.SetMachineDisabled(true);
        }
    }

    private void EnableMachines(int index)
    {
        DisableAllMachines();
        foreach (MachineGroup mg in sequence)
        {
            if (mg.groupIndex != index) continue;

            foreach (Machine machine in mg.enabledMachines)
            {
                machine.SetMachineDisabled(false);
            }
        }
    }
}
