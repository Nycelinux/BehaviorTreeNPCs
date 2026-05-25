using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageDefenseSystem : MonoBehaviour
{
    public int barricadesBuilt;
    public bool attackActive;
    public int villageHealth =100;
    public int enemiesKilled;
    public static VillageDefenseSystem instance;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (attackActive && enemiesKilled >= 20 && villageHealth > 50)
            CompleteDefense();
    }

    void CompleteDefense()
    {
        attackActive = false;
        Quest quest = QuestManager.instance.activeQuests.Find(q => q.questID == QuestID.DefendVillage);

        if (quest != null)
            QuestManager.instance.CompleteQuest(quest);
        Debug.Log("Dorf erfolgreich verteidigt");
    }

}
