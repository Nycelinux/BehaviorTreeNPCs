using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Villager : NPC_Base, IInteractable
{
    public InteractType GetInteractType() => InteractType.NPC;
    public Transform player;
    public Transform homePoint;
    public Transform resourcePoint;
    public DialogueData villagerDialogue;
    public Ressourcetyp preferredResource;
    private bool autoGatherEnabled= false;
    private Blackboard blackboard;
    public bool hasConvai = false;
    public static bool convaiGlobalLock = false;

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

    public string BuildContext()
    {
        var context = GetComponent<ConvaiContextProvider>();
        return context != null ? context.BuildConvaiPrompt() : "You're a villager";
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
        Debug.Log("Interact: " + npcName);


        ConvaiContextProvider context = GetComponent<ConvaiContextProvider>();
        string convaiContext = context != null ? context.BuildConvaiPrompt() : "You're a villager";
        if (hasConvai)
        {
            Debug.Log("Starte Convai Dialog");
            if (convaiGlobalLock)
            {
                Debug.Log("Convai gesperrt");
                return;
            }
            if (ConvaiDialogueService.instance == null)
            {
                Debug.LogError("ConvaiDialogueService fehlt");
                return;
            }
            convaiGlobalLock = true;
            if (DialogueManager.instance == null)
            {
                Debug.LogError("DialogueManager.instance ist NULL");
                return;
            }

            
                ConvaiDialogueService.instance.StartConversation(this, BuildContext());
            
        
            Debug.Log("ContextProvider = " + GetComponent<ConvaiContextProvider>());
            Debug.Log("Context = " + convaiContext);
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
