using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsGTrigger : MonoBehaviour
{
    public InstructionsG dialogue
        ;

    public void TriggerInstructions()
    {
        FindObjectOfType<InstructionsGeologyManager>().StartInstructions(dialogue);
    }
}
