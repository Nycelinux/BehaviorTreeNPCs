using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqaudBlackboard
{
    public Transform sharedTarget;
    public bool isLeader;
    public Vector3 lastSeenPosition;
    public Vector3 rallyPoint;
    public Vector3 movePosition;
    public Vector3 forward;
    public Vector3 right;
    public bool isUnderAttack;
    public bool isInCombat;
    public bool retreating;
    public float squadFear;
    public float fear;
    public float health;
    public List<Blackboard> members = new();

}
