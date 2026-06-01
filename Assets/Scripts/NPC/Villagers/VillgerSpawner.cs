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
    private int nextWaypointIndex =0;

    [Header("Prefabs")]
    public GameObject guardPrefab;
    public GameObject vilPrefab;

    private List<Transform> houses = new List<Transform>();
    private HashSet<Transform> occupiedHouses = new HashSet<Transform>();
    private string[] problems =
    {
        "Seit dem letzten ANgriff wird mein Mann vermisst",
        "Wir brauchen Stein, u den Tempel zu verbessern.",
        "Wir brauchen mehr Nahrung",
        "Mein Sohn ist krank",
        "Mein Haus ist undicht, ich brauche Holz für die Reperatur",
    };

    private string[] names =
    {
        "Ix Chel",
        "Chaac",
        "Itzamná",
        "Kukulkan",
        "Pakal",
    };
    void Start()
    {
        FindHouse();
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