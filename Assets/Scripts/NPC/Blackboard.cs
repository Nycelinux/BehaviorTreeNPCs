using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Blackboard
{
    public bool isHungry;
    public bool isAfraid;
    public bool isAlerted;
    public bool isNight;
    public bool isHostile;
    public bool isLeader;

    public float fear;
    public float health;
    public float hunger;
    public float reputation;
    public float loyality;

    public Transform currentTarget;
    public Transform player;
    public NavMeshAgent navAgent;
    public squadRole role;
    public Vector3 lastKnownTargetPosition;
    public Vector3 forward;
    public Vector3 right;
    public Vector3 movePosition;
    public RessourceNode currentRessource;
}
