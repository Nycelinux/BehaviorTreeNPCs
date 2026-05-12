using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAttack : MonoBehaviour
{
    public static RatAttack instance;

    [Header("Wave Settings")]
    public int currentWave = 0;
    public int maxWaves = 5;
    float timeBetweenWaves = 25f;

    [Header("Scaling")]
    public int additionRatsPerWave = 3;
    public int ratsPerWave = 6;

    private bool attackRunning = false;

    private void Awake()
    {
        instance = this;
    }

    public void StartRatAttack()
    {
        if (attackRunning)
            return;
        attackRunning = true;
        currentWave = 0;
        StartCoroutine(RatWaveRoutine());
    }

    IEnumerator RatWaveRoutine()
    {
        DialogueUI.instance.ShowHint("Die Ratten überfallen das Dorf");
        while (currentWave < maxWaves)
        {
            currentWave++;
            int amount = ratsPerWave + (currentWave * additionRatsPerWave);
            EnemySpawner.instance.SpawnRats(amount);
            Debug.Log("Rattenwelle: " + currentWave + "startet mit "+ amount+ " Ratten!");
            NPCReaction[] villagers = FindObjectsOfType<NPCReaction>();
            foreach (var npc in villagers)
                npc.fear += 10f;
            
            yield return new WaitForSeconds(timeBetweenWaves);

        }
        attackRunning = false;

        DialogueUI.instance.ShowHint("Die Rattenplage wurde vertrieben");
        StoryManager.instance.StartStage(StoryStage.Diplomacy);
    }
}
