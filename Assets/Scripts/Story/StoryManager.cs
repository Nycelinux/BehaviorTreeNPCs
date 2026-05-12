using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public StoryStage currentStage;
    public static StoryManager instance;
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartStage(StoryStage.Arrival);
        //StartStage(StoryStage.TempleVisits);
    }
   
     public void StartStage(StoryStage stage)
    {
        currentStage = stage;
        Debug.Log(" Neue Storyphase " + stage);
        switch (stage)
        {
            case StoryStage.Arrival:
                StartArrivalStage();
                Debug.Log("Ankunft im Dorf");
                break;
            case StoryStage.Unrest:
                StartUnrestStage();
                Debug.Log("Unzufriedenheit steigt, Reputation sinkt");
                break;
            case StoryStage.TempleVisits:
                StartTempleVisitsStage();
                Debug.Log("Tempel besuchen und Priester kennenlernen");
                break;
            case StoryStage.FirstAttack:
                StartFirstAttackStage();
                break;
            case StoryStage.DefendVillage:
                StartDefendVillageStage();
                Debug.Log("Verteidigung organisieren");
                break;
            case StoryStage.SaveHarvest:
                StartSaveHarvestStage();
                Debug.Log("Ernte retten!");
                break;
            case StoryStage.GatherWood:
                StartGatherWoodStage();
                Debug.Log("Holz sammeln");
                break;
            case StoryStage.FindArtifact:
                StartFindArtifactStage();
                Debug.Log("Finde das Artefakt");
                break;
            case StoryStage.GainPriestTrust:
                StartGainPriestTrustStage();
                Debug.Log("Reputation beim Priester erhöhen");
                break;
            case StoryStage.PriestDecision:
                StartPriestDecisionStage();
                Debug.Log("TriggerPriestDecision");
                break;
            case StoryStage.RatAttack:
                StartRatAttackStage();
                Debug.Log("Die Ratten überrollen das Dorf");
                break;
            case StoryStage.Diplomacy:
                StartDiplomacyStage();
                Debug.Log("Verhandle mit dem feindlichen Dorf");
                break;
            case StoryStage.Engagement:
                StartEngagementStage();
                Debug.Log("Bitte um Erlaubnis für Verlobung");
                break;
            case StoryStage.FinalAttack:
                StartFinalAttackStage();
                Debug.Log("Der letzte Angriff startet!");
                break;
            case StoryStage.FinalDecision:
                StartFinalDecisionStage();
                Debug.Log("Die letzte Entscheidung steht an");
                break;
            case StoryStage.GoodEnding:
                StartGoodEndingStage();
                Debug.Log("Das Dorf ist gerettet!");
                break;
            case StoryStage.BadEnding:
                StartBadEndingStage();
                Debug.Log("Das Dorf ist verloren. Die armen Bewohner. Tja, vielleicht klappt es beim nächsten mal.");
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
        if (GameManager.instance.reputation >= 60)
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
        Debug.Log("Die Priesterin segnet das Dorf");
        NPCBuffSystem.instance.ApplyCalmBuff();
        StartStage(StoryStage.Diplomacy);
    }

    void CurseVillage()
    {
        Debug.Log("Die Priesterin verflucht das Dorf");
        EnemySpawner.instance.SpawnRats(5);
        ResourceManager.instance.ReduceResources();
        StartStage(StoryStage.RatAttack);
    }

    void StartArrivalStage()
    {
        Debug.Log("Du erreichst das Dorf");
        GameManager.instance.reputation = 40;
        NPCReaction[] villagers = FindObjectsOfType<NPCReaction>();
        foreach (var npc in villagers)
            npc.fear += 10f;
        DialogueUI.instance.ShowHint("Die Dorfbewohner wirken nervös. Vielleicht weiß die Priesterin mehr.");
        StartStage(StoryStage.TempleVisits);
    }

    void StartUnrestStage()
    {
        Debug.Log("Die Dorfbewohner sind unzufrieden");
        GameManager.instance.reputation -= 10;
        NPCReaction[] villagers = FindObjectsOfType<NPCReaction>();
        foreach (var npc in villagers)
        {
            npc.fear += 20f;
            npc.loyality -= 10f;
        }
        EnemySpawner.instance.SpawnEnemies(2);
    }
    void StartTempleVisitsStage()
    {
        Debug.Log("Besuche den Tempel.");
        CreateQuest(QuestID.VisitTemple, "Besuche die Priesterin", "Die Bewohner schicken ddich zum Tempel", 1, Quest.QuestType.Investigate);
    }

    void StartFirstAttackStage()
    {
        Debug.Log("Feinde greifen das Dorf an.");
        EnemySpawner.instance.SpawnEnemies(5);
        NPCReaction[] villagers = FindObjectsOfType<NPCReaction>();
        foreach (var npc in villagers)
            npc.fear += 30f;
        CreateQuest(QuestID.RatAttack, "Verteidige das Dorf", "Besiege die Angreifer", 5, Quest.QuestType.Kill);

    }

    void StartDefendVillageStage()
    {
        Debug.Log("Organissier die Verteidigung");
        NPC_Guard[] guards = FindObjectsOfType<NPC_Guard>();
        
        foreach (var guard in guards)
        {
            Health hp = guard.GetComponent<Health>();
            if(hp != null)
                hp.maxHealth += 20;
            
        }
            

        CreateQuest(QuestID.GatherWood, "Verteidige das Dorf", "Sammle Holz für Barrikaden", 15, Quest.QuestType.Build);

    }

    void StartSaveHarvestStage()
    {
        Debug.Log("Die Ernte ist bedroht.");
        ResourceManager.instance.ReduceResources();
        CreateQuest(QuestID.SaveHarvest, "Rette die Ernte", "Beschütze die Felder vor Ratten", 10, Quest.QuestType.Defend);

    }

    void StartGatherWoodStage()
    {
        Debug.Log("Sammle Holz");
        CreateQuest(QuestID.GatherWood, "Holz sammeln", "Sammle Holz für die Reperatur", 10, Quest.QuestType.Collect);
        if (GameManager.instance.reputation > 60)
        {
            Debug.Log("Die Dorfbewohner helfen beim Sammeln");
            NPC_Villager[] villagers = FindObjectsOfType<NPC_Villager>();
            foreach (var villager in villagers)
            {
                villager.EnableAutoGather();
            }
        }
    }

    void StartFindArtifactStage()
    {
        Debug.Log("Finde das Artefakt.");
        //DungeonManager.instance.OpenTempleDungeon();
        CreateQuest(QuestID.FindArtifact, "Finde das Artifakt", "Finde das Artifakt in dem Tempel", 1, Quest.QuestType.Talk);

    }

    void StartGainPriestTrustStage()
    {
        Debug.Log("Gewinne das Vertrauen der Priesterin.");
        if (GameManager.instance.reputation >= 50)
        {
            DialogueUI.instance.ShowHint("Die Priesterin scheint Vertrauen zu fassen");
        }
        else
        {
            DialogueUI.instance.ShowHint("Die Priesterin misstraut dir weiterhin");

        }
    }

    void StartRatAttackStage()
    {
        Debug.Log("Ratten zerstören die Ernte");
        RatAttack.instance.StartRatAttack();
        EnemySpawner.instance.SpawnRats(25);// nötig?

        NPCReaction[] villagers = FindObjectsOfType<NPCReaction>();
        foreach (var npc in villagers)
        {
            npc.fear += 50f;
        }
        CreateQuest(QuestID.RatAttack, "Stoppe die Rattenplage", "Besiege die Ratten, bevor die Ernte zerstört wird", 25, Quest.QuestType.Kill);

    }

    void StartDiplomacyStage()
    {
        Debug.Log("Kontaktiere das Nachbardorf");
        CreateQuest(QuestID.Diplomacy, "Reise zum Nachbardorf", "Verhandle ein Bündnis", 1, Quest.QuestType.Talk);

    }

    void StartEngagementStage()
    {
        Debug.Log("Verlobung verhandeln");
        if (GameManager.instance.reputation > 70)
        {
            StartStage(StoryStage.GoodEnding);
        }
        else
        {
            StartStage(StoryStage.FinalAttack);

        }
    }

    void StartFinalAttackStage()
    {
        Debug.Log("Großer Angriff beginnt");
        EnemySpawner.instance.SpawnEnemies(30);
        NPCReaction[] villagers = FindObjectsOfType<NPCReaction>();
        foreach (var npc in villagers)
        {
            npc.fear += 80f;
        }
        CreateQuest(QuestID.RatAttack, "Überlebe den Angriff", "Verteidige das Dorf", 30, Quest.QuestType.Survive);

    }

    void StartFinalDecisionStage()
    {
        Debug.Log("Wurde das Dorf gerettet?");
        if (GameManager.instance.reputation >= 70)
        {
            StartStage(StoryStage.GoodEnding);
        }
        else
        {
            StartStage(StoryStage.BadEnding);

        }
    }

    void StartGoodEndingStage()
    {
        Debug.Log("Das Dorf floriert");
        NPCReaction[] villagers = FindObjectsOfType<NPCReaction>();
        foreach (var npc in villagers)
        {
            npc.loyality += 100f;
            npc.fear = 0f;
        }
    }

    void StartBadEndingStage()
    {
        Debug.Log("Das Dorf ist verloren");
        NPCReaction[] villagers = FindObjectsOfType<NPCReaction>();
        foreach (var npc in villagers)
        {
            npc.loyality += 0f;
            npc.fear = 100f;
        }
        EnemySpawner.instance.SpawnEnemies(30);
    }

    void StartPriestDecisionStage()
    {
        Debug.Log("Die Priesterin entscheidet über das Dorf");
        TriggerPriestDecision();
    }

    void CreateQuest(QuestID id, string title, string desc, int amount, Quest.QuestType type)
    {
        if (QuestManager.instance.HasQuest(id))
            return;
        Quest quest = new Quest
        {
            questID = id,
            questName = title,
            description = desc,
            requiredAmount = amount,
            currentAmount = 0,
            questType = type
        };
        QuestManager.instance.AddQuest(quest);
    }
}
