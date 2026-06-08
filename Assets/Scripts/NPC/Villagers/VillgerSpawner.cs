using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillgerSpawner : MonoBehaviour
{
    public int guardCount = 1;
    public int vilCount = 3;
    public LayerMask villageLayer;
    public Transform player;
    public GameObject convaiVillagerPrefab;
    private int nextWaypointIndex =0;

    [Header("Villager Dialogue")]
    public DialogueData[] villagerDialogue;

    [Header("Prefabs")]
    public GameObject guardPrefab;
    public GameObject vilPrefab;

    private List<Transform> houses = new List<Transform>();
    private HashSet<Transform> occupiedHouses = new HashSet<Transform>();
    private string[] problems =
    {
        "Seit dem letzten ANgriff wird mein Mann vermisst",
        "Wir brauchen Stein, u den Tempel zu verbessern.",
        "Mein Sohn ist krank",
        "Mein Haus ist undicht, ich brauche Holz für die Reperatur",
    };

    private string[] names =
    {
        "Itzamná",
        "Kukulkan",
        "Pakal",
    };
    void Start()
    {
        FindHouse();
        SpawnConvaiVillager();
        SpawnVillager();
        SpawnGuard();
    }

    void FindHouse()
    {
        houses.Clear();
        GameObject[] foundHouses = GameObject.FindGameObjectsWithTag("Haus");
        Debug.Log("Häuser gefunden: " + foundHouses.Length);
        foreach(GameObject house in foundHouses)
        {
            houses.Add(house.transform);
            Debug.Log("Home registriert: " + house.name);
        }

    }

    void SpawnGuard()
    {
        Debug.Log(" Spawned guard count: " + guardCount);
        for (int i = 0; i < guardCount; i++)
        {
            Debug.Log("Spawn guard " + i);
            SpawnNPC(guardPrefab, true);
        }

    }

    void SpawnVillager()
    {
        for (int i = 0; i < vilCount; i++)
            SpawnNPC(vilPrefab, false);
    }

    void SpawnConvaiVillager()
    {
        if(convaiVillagerPrefab == null)
        {
            Debug.LogWarning("convai villager prefab fehlt ");
            return;
        }

        Vector3 spawnPosition = GetVillagePos();
        GameObject npc = Instantiate(convaiVillagerPrefab, spawnPosition, Quaternion.identity);
        NPC_Villager villager = npc.GetComponent<NPC_Villager>();

        if(villager != null)
        {
            villager.hasConvai = true;
            villager.player = player;
            villager.npcName = "Ix Chel";
            villager.PersonalProblem = " Mein Mann wird seit dem letzten ANgriff vermisst";
            villager.preferredResource = Ressourcetyp.Food;
        }

        AssignHome(npc.transform);
        Debug.Log("Convai Villager Ix Chel gespawnt");


    }

    void AssignHome(Transform npc)
    {
        NPC_Villager villager = npc.GetComponent<NPC_Villager>();
        NPC_Guard guard = npc.GetComponent<NPC_Guard>();
        Transform homeTrans = null;
        foreach(Transform house in houses)
        {
            if (!occupiedHouses.Contains(house))
            {
                occupiedHouses.Add(house);
                homeTrans = house;
                Debug.Log( npc.name + " bekommt house " + house.name);
                break;
            }
        }

        if(homeTrans == null)
        {
            GameObject fallback = new GameObject(npc.name + "_Home");
            fallback.transform.position = npc.position;
            homeTrans = fallback.transform;
            Debug.LogWarning("Kein freies Haus gefunden für " + npc.name);
        }
        NavMeshHit hit;
        if(NavMesh.SamplePosition(homeTrans.position, out hit, 5f, NavMesh.AllAreas))
        {
            homeTrans.position = hit.position;
        }
        else
        {
            Debug.LogError("Home ist nicht auf NavMesh: " + homeTrans.name);
        }

        if (villager != null)
        {
            villager.homePoint = homeTrans;
            Debug.Log(villager.name + " assigned home: " + homeTrans.name + " at " + homeTrans.position);
        }
            
        if (guard != null)
        {
            guard.homePoint = homeTrans;
            Debug.Log(guard.name + " assigned home: " + homeTrans.name + " at " + homeTrans.position);

        }

    }

    void SetupVillager(GameObject npc)
    {
        NPC_Villager villager = npc.GetComponent<NPC_Villager>();
        if (villager == null)
            return;
        villager.player = player;
        villager.npcName = names[Random.Range(0, names.Length)];
        villager.PersonalProblem = problems[Random.Range(0, problems.Length)];
        villager.preferredResource = (Ressourcetyp) Random.Range(0, System.Enum.GetValues(typeof(Ressourcetyp)).Length);
        villager.dialoguePool.Clear();
        int villagerType = Random.Range(0, 5);

        switch (villagerType)
        {
            case 0:
                villager.npcName = "Ah Kin";
                villager.PersonalProblem = "Die Maisfelder liefern zu wenig Nahrung";
                break;
            case 1:
                villager.npcName = "Itzamná";
                villager.PersonalProblem = "Der Fluss liefert weniger Fisch";
                break;
            case 2:
                villager.npcName = "Chak Tok";
                villager.PersonalProblem = "Der Tempel bringt mehr Stein";
                break;
            case 3:
                villager.npcName = "Yax Tun";
                villager.PersonalProblem = "Die Wälder werden gefährlicher";
                break;
            case 4:
                villager.npcName = "Pakal";
                villager.PersonalProblem = "Die Händler verlangen höhere Preise";
                break;

        }

        if(villagerDialogue.Length > 0)
        {
            int amount = Mathf.Min(4, villagerDialogue.Length);
            List<DialogueData> available = new List<DialogueData>(villagerDialogue);
            for(int i = 0; i<amount; i++)
            {
                int index = Random.Range(0, available.Count);
                villager.dialoguePool.Add(available[index]);
                available.RemoveAt(index);
            }

            villager.villagerDialogue = villager.dialoguePool[0];
        }
    }
    void SpawnNPC(GameObject prefab, bool isGuard)
    {
        Vector3 spawnPos = GetVillagePos();
        if(spawnPos == Vector3.zero)
        {
            Debug.LogError("kein gültiger NavMesh Punkt, spawn abgebrochen");
            return;
        }

        GameObject npc = Instantiate(prefab, spawnPos, Quaternion.identity);
       
        NavMeshAgent navAgent = npc.GetComponent<NavMeshAgent>();

        if (navAgent != null)
        {
            NavMeshHit hit;
            if(NavMesh.SamplePosition(spawnPos, out hit, 5f, NavMesh.AllAreas))
            {
                npc.transform.position = hit.position;
                navAgent.Warp(hit.position);
            }
           
        } 
        if (!isGuard)
            SetupVillager(npc);
        AssignHome(npc.transform);

        NPC_Guard guard = npc.GetComponent<NPC_Guard>();

        if (isGuard && guard != null)
        {
            AssignGuardWaypoints(npc);
        }

    }

    Vector3 GetVillagePos()
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-25f, 25f),
                0f,
                Random.Range(-25f, 25f)
            );

            Vector3 rawPos = transform.position + randomOffset;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(rawPos, out hit, 20f, NavMesh.AllAreas))
            {
                Debug.Log("NavMesh Spawn gefunden: " + hit.position);
                return hit.position;
            }
        }

        Debug.LogWarning("Kein gültiger NavMesh Spawnpunkt gefunden!");
        NavMeshHit fallbackHit;
        if (NavMesh.SamplePosition(transform.position, out fallbackHit, 50f, NavMesh.AllAreas))
        {
            Debug.Log("Fallback position: " + fallbackHit.position);
            return fallbackHit.position;
        }

        return Vector3.zero;
    }

    void AssignGuardWaypoints( GameObject npc)
    {
        NPC_Guard guard = npc.GetComponent<NPC_Guard>();
        if (guard == null)
            return;
        GameObject[] allWaypointss = GameObject.FindGameObjectsWithTag("Waypoint");
        if(allWaypointss.Length < 2)
        {
            Debug.LogError(" zu wenig WAypoins in der Szene");
            return;

        }

        List<Transform> assigned = new List<Transform>();

        int amount = Mathf.Min(2, allWaypointss.Length); 

        for (int i = 0; i < amount; i++)
        {
            int idx = (nextWaypointIndex + i) % allWaypointss.Length;
            assigned.Add(allWaypointss[idx].transform);
        }

        nextWaypointIndex = (nextWaypointIndex + amount) % allWaypointss.Length;

        guard.wayPoints = assigned.ToArray();
        Debug.Log(guard.name + " bekam " + guard.wayPoints.Length + " Waypoints");
    }
}       