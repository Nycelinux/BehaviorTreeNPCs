using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI artifactText;
    public int food;
    public int stone;
    public int wood;
    public int artifact;
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
        foodText.text = "food: " + food;
        stoneText.text = "stone: " + stone;
        woodText.text = "wood: " + wood;  
        artifactText.text = "artifact: " + artifact;  
        if(playerHealth != null)
        {
            playerHealthText.text = "HP: " + playerHealth.GetHealth();
        }
    }

    public void AddResources(Ressourcetyp type, int amount)
    {
        switch (type)
        {
            case Ressourcetyp.Food:
                food += amount;
                break;
            case Ressourcetyp.Stone:
                stone += amount;
                break;
            case Ressourcetyp.Wood:
                wood += amount;
                break;
            /*case Ressourcetyp.FaithArtifact:
                artifact += amount;
                break;*/
        }
    }
}
