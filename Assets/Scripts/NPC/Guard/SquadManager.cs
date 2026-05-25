using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadManager : MonoBehaviour
{
    public static SquadManager instance;
    private SqaudBlackboard squad = new SqaudBlackboard();
    void Awake()
    {
        instance = this;
    }

    public void Register(Blackboard blackboard)
    {
        if (!squad.members.Contains(blackboard))
            squad.members.Add(blackboard);
    }

    public void AssignRoles()
    {
        foreach (var member in squad.members) {

            if (member.health < 30)
                member.role = squadRole.Support;
            else if (member.fear > 70)
                member.role = squadRole.Scout;
            else
                member.role = squadRole.DPS;
        }
    }

    public void SetCombat(Transform target)
    {
        squad.sharedTarget = target;
        squad.isInCombat= true;

        foreach (var member in squad.members)
            member.currentTarget = target;
    }

    public void UpdateSquadVectors()
    {
        Vector3 avgPos = Vector3.zero;
        foreach (var member in squad.members)
            avgPos += member.navAgent.transform.position;
        avgPos /= squad.members.Count;
        foreach (var member in squad.members)
        {
            Vector3 dir = (member.navAgent.transform.position - avgPos).normalized;
            member.forward = dir;
            member.right = Vector3.Cross(Vector3.up, dir);
        }
    }
    
}
