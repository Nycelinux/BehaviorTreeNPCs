using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckNight : Node
{
    public override NodeState Evaluate()
    {
        if (GameManager.instance.isNight)
            return NodeState.SUCCESS;
        return NodeState.FAILURE;

    }
}
