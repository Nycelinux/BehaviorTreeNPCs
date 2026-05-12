using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestRitualNode : Node
{
    public override NodeState Evaluate()
    {
        if (UIManager.instance.resources >= 10)
        {
            UIManager.instance.resources -= 10;
            Debug.Log("Ritual durchgeführt");
            NPCBuffSystem.instance.ApplyCalmBuff();
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
