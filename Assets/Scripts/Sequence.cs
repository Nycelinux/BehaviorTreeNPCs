using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    private List<Node> children;
    public Sequence(Blackboard blackboard,List<Node> children): base(blackboard)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        for (int i = 0; i < children.Count; i++)
        {
            NodeState result = children[i].Evaluate();
            if (result == NodeState.FAILURE)
            {
                state = NodeState.FAILURE;
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
    /*public override NodeState Evaluate()
    {
        bool anyRunning = false;

        foreach (var child in children)
        {
            NodeState result = child.Evaluate();

            if (result == NodeState.FAILURE)
                return NodeState.FAILURE;

            if (result == NodeState.RUNNING)
                anyRunning = true;
        }

        return anyRunning ? NodeState.RUNNING : NodeState.SUCCESS;
    }*/
}
