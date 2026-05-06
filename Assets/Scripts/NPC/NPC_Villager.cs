using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Villager : MonoBehaviour
{
    public Transform player;
    public Transform homePoint;
    public Transform resourcePoint;

    private Node root;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        NPCReaction reaction = GetComponent<NPCReaction>();
        root = new Selector(new List<Node> {
        new CollectRessource(agent, reaction),
        new HostileNode(agent,player),
        new SleepNode(agent,homePoint),
        new FollowNode(agent,player)
        }) ;
    }

    void Update()
    {
        root.Evaluate();
    }
}
