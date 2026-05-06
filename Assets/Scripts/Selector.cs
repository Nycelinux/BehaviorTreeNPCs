using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    private List<Node> children;
    public Selector(List<Node> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        foreach (Node child in children)
        {
            switch (child.Evaluate())
            {
                case NodeState.SUCCESS:
                    state = NodeState.SUCCESS;
                    return state;

                case NodeState.RUNNING:
                    state = NodeState.RUNNING;
                    return state;

            }
        }
        state = NodeState.FAILURE;
        return state;
    }
}
