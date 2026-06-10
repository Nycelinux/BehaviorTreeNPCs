using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Villager : NPC_Base, IInteractable
{
    public Transform player;
    public Transform homePoint;
    public Transform resourcePoint;
    public DialogueData villagerDialogue;
    public Ressourcetyp preferredResource;
    private bool autoGatherEnabled= false;
    public bool hasConvai= false;
    private Blackboard blackboard;
    [TextArea]
    public string PersonalProblem;

    [Header("Wander")]
    public float wanderRadius = 8f;

    [Header("Personality")]
    public string npcName;
    private NPCReaction reaction;

    [Header("Dialogue Pool")]
    public List<DialogueData> dialoguePool = new List<DialogueData>();

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
            

            new WanderNode(blackboard, navAgent,transform.position,wanderRadius,player),
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

    public void Interact(Vector3 hitPoint)
    {
        Debug.Log("=== NPC_Villager.Interact() ===");
        Debug.Log("NPC Name: " + npcName);
        Debug.Log("hasConvai: " + hasConvai);
        Debug.Log("dialoguePool Count: " + dialoguePool.Count);
        Debug.Log("villagerDialogue: " + villagerDialogue);

        ConvaiContextProvider context = GetComponent<ConvaiContextProvider>();
        string convaiContext = context != null ? context.BuildConvaiPrompt() : "You're a villager";
        if (hasConvai)
        {
            Debug.Log("Starte Convai Dialog");

            if (DialogueManager.instance == null)
            {
                Debug.LogError("DialogueManager.instance ist NULL");
                return;
            }

            DialogueManager.instance.StartConvaiDialogue(this, convaiContext);
                return;
        }

        Debug.Log(npcName +" dialoguePool count = " +dialoguePool.Count);
        if (dialoguePool != null && dialoguePool.Count > 0)
        {
            DialogueData randomDialogue = dialoguePool[Random.Range(0, dialoguePool.Count)];
            Debug.Log("Öffne Dialog: " + randomDialogue.name);
            DialogueManager.instance.StartDialogue(randomDialogue, gameObject);
            return;
        }
        
        
        if(villagerDialogue != null)
        {
            Debug.Log("Öffne Fallback Dialog: " + villagerDialogue.name);
            DialogueManager.instance.StartDialogue(villagerDialogue, gameObject);
            return;
        }
        Debug.LogError("KEIN DIALOG GEFUNDEN FÜR " + npcName);
    }
}
