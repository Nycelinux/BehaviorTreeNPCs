using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public int currentStage = 0;
    public static StoryManager instance;
    void Awake()
    {
        instance = this;
    }

    public void AdvanceStory()
    {
        currentStage++;
        Debug.Log("Story Fortschritt: " + currentStage);
        TriggerStageEvent();
    }
     void TriggerStageEvent()
    {
        switch (currentStage)
        {
            case 1:
                Debug.Log("Ankunft im Dorf");
                break;
            case 2:
                Debug.Log("Ankunft im Dorf");
                break;
            case 3:
                Debug.Log("Ankunft im Dorf");
                break;
            case 4:
                SpawnEnemyWave();
                break;
            case 5:
                Debug.Log("Verteidigung organisieren");
                break;
            case 6:
                Debug.Log("Ernte retten!");
                break;
            case 7:
                Debug.Log("Holz sammeln");
                break;
            case 8:
                Debug.Log("Finde das Artefakt");
                break;
            case 9:
                Debug.Log("Reputation beim Priester erhöhen");
                break;
            case 10:
                Debug.Log("TriggerPriestDecision");
                break;
        }
    }

    void SpawnEnemyWave()
    {
        Debug.Log("Enemy greift an!");
        EnemySpawner.instance.SpawnEnemies(3);
    }

    void TriggerPriestDecision()
    {
        if (GameManager.instance.reputation > 60)
        {
            BlessVillage();
        }
        else
        {
            CurseVillage();
        }
    }

    void BlessVillage()
    {
        Debug.Log("Dorf gesegnet");
        NPCBuffSystem.instance.ApplyCalmBuff();
    }

    void CurseVillage()
    {
        Debug.Log("Fluch aktiviert");
        EnemySpawner.instance.SpawnRats(5);
        ResourceManager.instance.ReduceResources();
    }
}
