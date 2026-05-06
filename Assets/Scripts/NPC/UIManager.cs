using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI resourceText;
    public int resources;
    public TextMeshProUGUI reputationText;
    public TextMeshProUGUI playerHealthText;
    public static UIManager instance;
    public Health playerHealth;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        reputationText.text = "Reputation: " + GameManager.instance.reputation;  
        resourceText.text = "Resource: " + resources;  
        if(playerHealth != null)
        {
            playerHealthText.text = "HP: " + playerHealth.GetHealth();
        }
    }

    public void AddResources(int amount)
    {
        resources += amount;
    }
}
