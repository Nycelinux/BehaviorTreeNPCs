using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    private List<Node> children;
    public Sequence(List<Node> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        bool anyRunning = false;
        foreach (Node child in children)
        {
            switch (child.Evaluate())
            {
                case NodeState.FAILURE:
                    state = NodeState.FAILURE;
                    return state;

                case NodeState.RUNNING:
                    anyRunning = true;
                    break;

            }
        }
        state = anyRunning? NodeState.RUNNING:NodeState.SUCCESS;
        return state;
    }
}
