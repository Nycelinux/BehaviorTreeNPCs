using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    protected Blackboard blackboard;
    public enum NodeState { SUCCESS, FAILURE, RUNNING}
    public NodeState state;
    public Node(Blackboard blackboard) {
        this.blackboard = blackboard;
    }    
    public abstract NodeState Evaluate();
}
