using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadRoleDecisionNode : Node
{
    public SquadRoleDecisionNode(Blackboard blackboard) : base(blackboard)
    {
    }

    public override NodeState Evaluate()
    {
        if(blackboard == null)
            return NodeState.FAILURE;
        float health = blackboard.health;
        float fear = blackboard.fear;

        if (blackboard.isLeader)
            blackboard.role = squadRole.Tank;
        else if (health < 35 || fear > 70)
            blackboard.role = squadRole.Support;
        else
            blackboard.role = squadRole.DPS;

        return NodeState.SUCCESS;
    }
}
