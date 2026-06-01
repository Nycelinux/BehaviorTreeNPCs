using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Villager : NPC_Base
{
    public Transform player;
    public Transform homePoint;
    public Transform resourcePoint;
    public Ressourcetyp preferredResource;
    private bool autoGatherEnabled= false;
    private Blackboard blackboard;
    [TextArea]
    public string PersonalProblem;

    [Header("Wander")]
    public float wanderRadius = 8f;

    [Header("Personality")]
    public string npcName;
    private NPCReaction reaction;

    protected override void Start()
    {

        base.Start();
        
        if(navAgent == null)
        {
            Debug.LogError(npcName + "hat keinen NavMeshAgent");
            return;
        }
        blackboard = new Blackboard();
        blackboard.navAgent = navAgent;
        blackboard.reputation = GameManager.instance.reputation;
        blackboard.player = player;
        if (homePoint == null)
        {
            GameObject home = new GameObject(npcName + "_Home");
            home.transform.position = transform.position;
            homePoint = home.transform;
        }
        reaction = GetComponent<NPCReaction>();
        Debug.Log(npcName + " HomePoint = " + homePoint);
        BuildTree();
        SetInitialized();
    }

    protected override void BuildTree()
    {
        root = new Selector(blackboard, new List<Node>
        {
            new Sequence(blackboard, new List<Node>
            {
                new CheckNight(blackboard),
                new SleepNode(blackboard,navAgent,homePoint),
            }),
            new Sequence(blackboard, new List<Node>
            {
                new HostileNode(blackboard,navAgent,player)

            }),
            new Sequence(blackboard, new List<Node>
            {
                new CollectRessource(blackboard,navAgent, reaction, this),
            }),
            new Sequence(blackboard, new List<Node>
            {
                new FollowNode(blackboard,navAgent,player)

            }),
            

            new WanderNode(blackboard, navAgent,transform.position,wanderRadius),
        }); ;
        
    }
   
    public void EnableAutoGather()
    {
        autoGatherEnabled = true;
        Debug.Log(name + " hilft beim sammeln");
    }

    public bool CanAutoGather()
    {
        return autoGatherEnabled;
    }
}
