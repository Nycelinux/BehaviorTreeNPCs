using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDialogue : Node
{
    public CheckDialogue(Blackboard blackboard) : base(blackboard)
    {
    }

    public override NodeState Evaluate()
    {
        if (blackboard == null)
            return NodeState.FAILURE;
        NPC_Villager villager = blackboard.self.GetComponent<NPC_Villager>();

        if (villager != null && villager.IsInDialogue)
            return NodeState.SUCCESS;

        return NodeState.FAILURE;
    }
}
