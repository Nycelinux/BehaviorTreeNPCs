using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    private List<Node> children;
    public Selector(Blackboard blackboard,List<Node> children): base(blackboard)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        for (int i = 0; i< children.Count; i++)
        {
            NodeState result = children[i].Evaluate();
            if (result ==  NodeState.SUCCESS)
            {
                state = NodeState.SUCCESS;
                return state;
            }

            if (result == NodeState.RUNNING)
            {
                state = NodeState.RUNNING;
                return state;
            }
        }
        state = NodeState.FAILURE;
        return state;
    }
}
