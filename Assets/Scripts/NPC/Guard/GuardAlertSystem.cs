using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAlertSystem : MonoBehaviour
{
    public static GuardAlertSystem instance;
    public List<Blackboard> guardBoards = new List<Blackboard>();
    void Awake()
    {
        instance = this; 
    }

    public void Register(Blackboard blackboard)
    {
        if (!guardBoards.Contains(blackboard))
            guardBoards.Add(blackboard);
    }

    public void AlertAll(Transform target)
    {
        foreach(var blackboard in guardBoards)
        {
            blackboard.currentTarget=target;
            blackboard.isAlerted=true;
        }
    }
}
