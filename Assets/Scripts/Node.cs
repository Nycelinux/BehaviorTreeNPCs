using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    public enum NodeState { SUCCESS, FAILURE, RUNNING}
    public NodeState state;
    public abstract NodeState Evaluate();
}
