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
    private bool iinitialized =false;



     protected override void Awake()
    {
        base.Awake();
        blackboard = new Blackboard();
       
        blackboard.navAgent = navAgent;
        blackboard.player = player;
        blackboard.health = GetComponent<Health>() != null ? GetComponent<Health>().currentHealth : 100f;
        blackboard.fear = 0f;
        blackboard.hunger = 0f;
        blackboard.reputation = 50f;
        blackboard.loyality = 50f;
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
      

        if (GuardAlertSystem.instance != null)
        GuardAlertSystem.instance.Register(blackboard);
        SquadManager.instance?.Register(blackboard);

    }
    public Blackboard GetBlackboard()
    {
        return blackboard;
    }

    protected override void Start()
    {
        base.Start();
        Debug.Log($"{name} Waypoints beim Start: {(wayPoints == null ? 0 : wayPoints.Length)}");

        if (wayPoints == null || wayPoints.Length < 2)
        {
            Debug.LogWarning($"{name}: keine gültigen Waypoints - Patrol deaktiviert");
        }
        Debug.Log(name + " hat Waypoints: " + (wayPoints == null ? 0 : wayPoints.Length));
        InitBlackboardAndTree();

    }

    protected override void Update()
    {
        Health hp = GetComponent<Health>();
        if (hp != null)
            blackboard.health = hp.currentHealth;
        base.Update();
    }

    public void InitBlackboardAndTree()
    {
        if (iinitialized) return;
        iinitialized = true;
        if (blackboard == null || navAgent == null)
        {
            Debug.LogError($"{name}: Blackboard oder NavAgent noch NULL (Init zu früh?)");
            return;
        }

        BuildTree();

        if (root == null)
        {
            Debug.LogError($"{name}:  Root ist NULL , BUildTree fehlgeschlagen");
            return;
        }
      
        initialized = true;
        Debug.Log("{name}: GuardTree Build erfolgreich!");

    }
    protected override void BuildTree()
    {
        if (blackboard == null || navAgent == null)
        {
            Debug.LogError("Guard Blackboardoder NavAgent felt");
            return;
        }
        if (navAgent == null)
        {
            Debug.LogError($"{name}: NavAgent NULL");
            return;
        }

        if (wayPoints == null || wayPoints.Length < 2)
        {
            Debug.LogWarning($"{name}: keine Patrol Waypoints - Patrol wird deaktiviert");
        }

        root = new Selector(blackboard,new List<Node>
        {
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
            new PatrolNode(blackboard,navAgent,wayPoints),
            new Sequence(blackboard,new List<Node>
            {
                new CheckNight(blackboard),
                new SleepNode(blackboard,navAgent,homePoint),
                
            }),
           
            
        });
    }
}

// Reputation beeinflusst Angst, Npcs helfen sich gegenseitig (heilen, fliehen gemeinsam) als Erweiterung
