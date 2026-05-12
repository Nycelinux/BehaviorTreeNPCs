using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public string npcName;
    [TextArea(3, 6)]
    public string dialogueText;
    public List<DialogueChoice> choices;
    public Sprite portrait;
 
}
