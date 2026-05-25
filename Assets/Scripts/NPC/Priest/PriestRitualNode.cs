using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestRitualNode : Node
{
    public PriestRitualNode(Blackboard blackboard) : base(blackboard) { }
    public override NodeState Evaluate()
    {
        if (UIManager.instance == null)
            return NodeState.FAILURE;
        bool hasResource = UIManager.instance.wood >= 10 && UIManager.instance.food >= 10 && UIManager.instance.stone >= 10;
        if (!hasResource)
            return NodeState.FAILURE;
        UIManager.instance.wood -= 10;
        UIManager.instance.food -= 10;
        UIManager.instance.stone -= 10;
        Debug.Log("Ritual durchgeführt");
        NPCBuffSystem.instance?.ApplyCalmBuff();
        return NodeState.SUCCESS;
    }
}
