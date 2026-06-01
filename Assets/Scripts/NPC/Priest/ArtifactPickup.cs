using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPickup : MonoBehaviour
{
    private bool collected = false;

    [Header("Artifact Values")]
    public float repReward = 10f;
    public float faiReward = 15f;
    public int artAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;
        if (other.CompareTag("Player"))
        {
            if (!other.CompareTag("Player"))
                return;
            if (collected)
                return;
            collected = true;
            Debug.Log("Artefakt eingesammelt");
            /*if (UIManager.instance != null)
                UIManager.instance.AddResources(Ressourcetyp.FaithArtifact, artAmount);*/
            if (QuestManager.instance != null)
                QuestManager.instance.ProgressQuest(QuestID.FindArtifact, 1);
            if (GameManager.instance != null)
                GameManager.instance.ChangeReputation(repReward);
            NPCReaction[] npcs = FindObjectsOfType<NPCReaction>();
            foreach(var npc in npcs)
            {
                npc.fear -= 10f;
                npc.loyality += 10f;
                npc.faith += faiReward;
            }

            if (DialogueUI.instance != null)
                DialogueUI.instance.ShowHint("Du hast das Artefakt gefunden");
            Destroy(gameObject);
        }
    }

}
