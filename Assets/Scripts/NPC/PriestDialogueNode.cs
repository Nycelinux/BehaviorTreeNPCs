using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestDialogueNode : Node
{
    public override NodeState Evaluate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float reputation = GameManager.instance.reputation;
            if(reputation < 30)
            {
                Debug.Log("Priesterin verflucht euch!");
                StoryManager.instance.AdvanceStory();
            }
            else
            {
                Debug.Log("Priesterin segnet euch!");
            }
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
