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

    [Header("Prefabs")]
    public GameObject guardPrefab;
    public GameObject vilPrefab;

    private List<HomePoint> freeHouse = new List<HomePoint>();
    private string[] problems =
    {
        "Seit dem letzten ANgriff wird mein Mann ´vermisst",
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
        HomePoint[] homes = FindObjectsOfType<HomePoint>();
        freeHouse.AddRange(homes);

    }

    void SpawnGuard()
    {
        for (int i = 0; i < guardCount; i++)
            SpawnNPC(guardPrefab, true);
    }

    void SpawnVillager()
    {
        for (int i = 0; i < vilCount; i++)
            SpawnNPC(vilPrefab, false);
    }

    void AssignHome(Transform npc)
    {
        Transform homeTrans = null;
        foreach(var home in freeHouse)
        {
            if (!home.isOccupied)
            {
                home.isOccupied = true;
                homeTrans = home.transform;
                break;
            }
        }

        if(homeTrans == null)
        {
            GameObject fallback = new GameObject(npc.name + "_Hoem");
            fallback.transform.position = npc.position;
            homeTrans = fallback.transform;
        }
        NPC_Villager villager = npc.GetComponent<NPC_Villager>();
        if (villager != null)
            villager.homePoint = homeTrans;
        NPC_Guard guard = npc.GetComponent<NPC_Guard>();
        if (guard != null)
            guard.homePoint = homeTrans;
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
        AssignHome(npc.transform);
        if (!isGuard)
            SetupVillager(npc);
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
}       