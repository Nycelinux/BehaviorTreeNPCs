using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueChoiceFactory
{
    public static List<DialogueChoice> GenerateBasicChoices()
    {
        return new List<DialogueChoice>
        {
            new DialogueChoice
            {
                choiceText = " Ich werde helfen",
                reputationChange = +5,
                relationShipEffect= new DialogueRelationShipEffect
                {
                    fearChange= -2,
                    friendshipChange= +10,
                    loyalityChange= +5,
                    trustChange= + 2,
                    respectChange= +2

                }
            },
            new DialogueChoice
            {
                choiceText = "Das ist nicht mein Problem",
                reputationChange = -5,
                relationShipEffect= new DialogueRelationShipEffect
                {
                    fearChange= +5,
                    friendshipChange= -10,
                    loyalityChange= -5,
                    trustChange= - 2,
                    respectChange= -2

                }
            },
            new DialogueChoice
            {
                choiceText = " Erzähl mir mehr",
                reputationChange = +1,
                relationShipEffect= new DialogueRelationShipEffect
                {
                    fearChange= -1,
                    friendshipChange= +3,
                    loyalityChange= +2,
                    trustChange= -1,
                    respectChange= -1

                }
            },
        };
    }
}
