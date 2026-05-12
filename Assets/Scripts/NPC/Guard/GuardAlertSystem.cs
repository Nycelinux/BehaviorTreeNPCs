using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAlertSystem : MonoBehaviour
{
    public static GuardAlertSystem instance;
    public List<GuardCombatNode> guards = new List<GuardCombatNode>();
    void Awake()
    {
        instance = this; 
    }

    public void Register(GuardCombatNode guard)
    {
        if (!guards.Contains(guard))
            guards.Add(guard);
    }

    public void AlertAll(Transform target)
    {
        foreach(var guard in guards)
        {
            guard.SetTarget(target);
        }
    }
}
