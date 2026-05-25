using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMenuUI : MonoBehaviour
{
    [Header("Menu")]
    public GameObject questPanel;
    [Header("Quest List")]
    public Transform contentParent;

    public GameObject questEntryPrefab;

    private bool isOpen = false;
    private List<GameObject> spawnedEntries = new List<GameObject>();

    public static QuestMenuUI instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        questPanel.SetActive(false);
        RefreshQuestList();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            ToggleQuestMenu();
    }

    public void ToggleQuestMenu()
    {
        if(DialogueUI.instance != null && DialogueUI.instance.dialoguePanel.activeSelf)
        {
            Debug.LogWarning("Questmenü kann während Dialog nicht geöffnet werden");
            return;
        }

        isOpen = !isOpen;
        questPanel.SetActive(isOpen);
        if (isOpen){
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void RefreshQuestList()
    {
        ClearQuestEntries();
        foreach (Quest quest in QuestManager.instance.activeQuests)
            CreateQuestEntry(quest);
    }

    void CreateQuestEntry(Quest quest)
    {
        GameObject entry = Instantiate(questEntryPrefab, contentParent);
        QuestEntryUI entryUI = entry.GetComponent<QuestEntryUI>();
        entryUI.Setup(quest);
        spawnedEntries.Add(entry);
    }

    void ClearQuestEntries()
    {
        foreach (GameObject entry in spawnedEntries)
            Destroy(entry);
        spawnedEntries.Clear();
    }
}
