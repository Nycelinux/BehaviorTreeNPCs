using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Guard : NPC_Base
{
    public Transform homePoint;
    public Transform[] wayPoints;
    public Transform player;
    public int attackDamage = 2;
    private Blackboard blackboard;

    protected override void Start()
    {
        base.Start();
        blackboard = new Blackboard();
        blackboard.navAgent = navAgent;
        blackboard.player = player;
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        if(wayPoints == null || wayPoints.Length == 0)
        {
            GameObject[] points = GameObject.FindGameObjectsWithTag("Waypoint");
            wayPoints = new Transform[points.Length]; 
            for (int i = 0; i < points.Length; i++)
                wayPoints[i] = points[i].transform;
            Debug.Log(" Waypoints gefunden " + wayPoints.Length);
        }
        if (GuardAlertSystem.instance != null)
            GuardAlertSystem.instance.Register(blackboard);
        SquadManager.instance?.Register(blackboard);

        BuildTree();
    }

    protected override void BuildTree()
    {
        
        root = new Selector(blackboard,new List<Node>
        {
            new Sequence(blackboard,new List<Node>
            {
                new CheckNight(blackboard),
                new SleepNode(blackboard,navAgent,homePoint)
            }),
             new Sequence(blackboard,new List<Node>
            {
                new LowHealthRetreatNode(blackboard,navAgent),
            }),
             new Sequence(blackboard,new List<Node>
            {
                new DetectEnemyNode(blackboard,this,navAgent),
                new SquadRoleDecisionNode(blackboard),
                new SquadPositioningNode(blackboard,navAgent),
                new SquadActionNode(blackboard,this, navAgent),

            }),
            new PatrolNode(blackboard,navAgent,wayPoints)
        });
    }
}

// Reputation beeinflusst Angst, Npcs helfen sich gegenseitig (heilen, fliehen gemeinsam) als Erweiterung
