using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("UI Prefab")]
    public bool isDead = false;
    public bool destroyOnDeath = true;
    public int maxHealth = 10;

    public int currentHealth;
    private FloatingHealthBar healthBar;
    private Animator animator;
    private GameOver gameOverManager;
    public System.Action<int, int> OnHealthChanged;
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Transform followTarget;
    [SerializeField] private Vector3 uiOffset =new Vector3(0,2f,0);

    void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        animator = GetComponent<Animator>();
        gameOverManager = FindAnyObjectByType<GameOver>();
        if (!CompareTag("Player"))
            SpawnHealthBar();
        
    }


    private void SpawnHealthBar()
    {
        if(healthBarPrefab == null)
        {
            Debug.LogWarning("No healthbbar prefab assigned on: " + gameObject.name);
            return;
        }
        

        GameObject bar = Instantiate(healthBarPrefab,transform);
        healthBar = bar.GetComponentInChildren<FloatingHealthBar>();
        Debug.Log(" Found healthbar: " + healthBar.gameObject.name);
        if (healthBar == null)
        {
            Debug.LogWarning("No floatinghealthbar found");
            return;
        }
        healthBar.SetTarget(followTarget != null? followTarget: transform,uiOffset);
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        Debug.Log(" Spawned at: " + bar.transform.position);
        Debug.Log("Target: " + transform.name);
    }

    private void Update()
    {
        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }


    // Update is called once per frame
    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if(healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
            Debug.Log(gameObject.name + " nimmt Schaden: " + amount);
            Debug.Log(gameObject.name + "HP: " + currentHealth);
            
        }
        
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
        NPCReaction reaction = GetComponent<NPCReaction>();
        if (reaction != null)
        {
            reaction.OnDamageTaken();
        }
        if (CompareTag("Player"))
        {
            if (GuardAlertSystem.instance != null)
                GuardAlertSystem.instance.AlertAll(transform);
        }

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " ist gestorben");
        if (animator != null)
        {
            animator.SetTrigger("Die");
            animator.SetBool("IsDead", true);
        }

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped=true;
        }

        if (CompareTag("Player"))
        {
            GetComponent<PlayerController>().enabled = false;
            if (gameOverManager != null)
                gameOverManager.GameOverPanel();
            GetComponent<PlayerCombat>().enabled = false;
            Debug.Log("Game Over!");
            return;
        }

        if (CompareTag("Enemy"))
        {
            QuestManager.instance.ProgressQuest(QuestID.FirstAttack, 1);
            Debug.Log("Enemy besiegt");
        }

            if (destroyOnDeath)
            Destroy(gameObject, 3f);
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    private void OnDestroy()
    {
        if (healthBar != null)
            Destroy(healthBar.gameObject);
    }
}
