using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsGeologyManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    private Queue<string> instructions;

    void Start()
    {
        instructions = new Queue<string>();
    }

    public void StartInstructions(InstructionsG dialogue)
    {
        Debug.Log("Start " + dialogue.name);
 //       nameText.text = dialogue.name;

        instructions.Clear();

        foreach (string instruction in dialogue.instructions)
        {
            instructions.Enqueue(instruction);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (instructions.Count == 0)
        {
        EndDialogue();
        return;
        }

        string instruction = instructions.Dequeue();
        //       Debug.Log(instruction);
        dialogueText.text = instruction;
    }

    /*
    public void DisplayPreviousSentence()
    {
        if (instructions.Count == 0)
        {
            EndDialogue();
            return;
        }

        else instructions.Count = 4;

        string instruction = instructions() ;
        //       Debug.Log(instruction);
        dialogueText.text = instruction;
    }
    */

    void EndDialogue()
    {
        Debug.Log("End of Conversation");
    }
}
