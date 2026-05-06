using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCReaction : MonoBehaviour
{
    [Header("Loyality")]
    public float loyality = 50f;

    [Header("Fear System")]
    public float fear = 0f;
    public float maxFear = 100f;
    public float fearIncreaseOnHit = 20f;
    public float fearDecreaseRate = 5f;

    [Header("Flee System")]
    public float fleeDistance = 10f;
    private Health health;
    private NavMeshAgent navAgent;
    private Transform player;
    private Animator animator;
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if(p != null)
        {
            player = p.transform;
        }
    }

     void Update()
    {
        ReduceFearOverTime();
        DetectNearbyEnemies();
        UpdateLoyality();

        if (fear > 60)
            SpreadFear();
    }

    public void OnDamageTaken()
    {
        IncreaseFear(fearIncreaseOnHit);
        DecideReaction();
    }
    
    void IncreaseFear(float amount)
    {
        fear += amount;
        fear = Mathf.Clamp(fear, 0, maxFear);
        Debug.Log(name + " Angst: " + fear);
    }

    void ReduceFearOverTime()
    {
        if(fear > 0)
        {
            fear -= fearDecreaseRate * Time.deltaTime;
            fear = Mathf.Clamp(fear, 0, maxFear);
        }
    }

    public void ReduceFearInstant(float amount)
    {
        fear -= fearDecreaseRate * Time.deltaTime;
        fear = Mathf.Clamp(fear, 0, maxFear);
        Debug.Log(name + " wurde beruhigt. Angst: " + fear);
    }

    void DecideReaction()
    {
        if(fear >= 70)
        {
            Flee();
        }
        else if (fear >= 30)
        {
            BecomeAggressive();
        }
        else
        {
            //ruhig bleiben
        }
    }

    void Flee()
    {
        if (player == null ||navAgent ==null) return;
        Vector3 direction = (transform.position - player.position).normalized;
        Vector3 randomOffset = Random.insideUnitSphere * 3f;
        randomOffset.y = 0;
        Vector3 fleePos = transform.position + direction * fleeDistance+randomOffset;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePos, out hit,5f, NavMesh.AllAreas))
        {
            navAgent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogWarning("Keine gültige NavMesh Position gefunden!");
        }
        if (animator != null)
        {
            animator.SetFloat("Speed", navAgent.speed * 1.5f);
        }

        if (GuardAlertSystem.instance != null)
            GuardAlertSystem.instance.AlertAll(transform);

        Debug.Log(name + " flieht!");
    }

    void BecomeAggressive()
    {
        Debug.Log(name + " wird aggressiv!");
        if (navAgent != null)
            navAgent.speed = 4.5f;
    }

    public float GetFear()
    {
        return fear;
    }

    void DetectNearbyEnemies()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, 5f);
        foreach(var e in enemies)
        {
            if (e.CompareTag("Enemy"))
                IncreaseFear(Time.deltaTime * 10f);
        }
    }
    void UpdateLoyality()
    {
        if (fear > 70) {
            loyality -= Time.deltaTime * 5f;
            GameManager.instance.reputation -= Time.deltaTime * 2f;
        }
        else 
        {
            loyality += Time.deltaTime * 2f;
        }
            

        loyality = Mathf.Clamp(loyality, 0, 100);
    }

    void SpreadFear()
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, 5f);
        foreach (var npc in nearby)
        {
            NPCReaction other = npc.GetComponent<NPCReaction>();
            if (other != null && other != this)
                other.IncreaseFear(Time.deltaTime * 20f);
        }
    }

    //vorläfige Dialoge 
    public string GetDialogue()
    {
        if (fear > 70)
            return "Ich habe Angst.. wir werden alle sterben!";
        if (loyality < 30)
            return "Ich trau euch nicht...";
        if (loyality > 70)
            return "Ich steh hinter euch, Bürgermeister";

        return "Alles ist ruhig";
    }

    private void OnMouseDown()
    {
        Debug.Log(GetDialogue());
    }
}
