using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsGeologyManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public string instruction;

    private Queue<string> instructions;
    private int totalInstructions;
    private int instructionsLeft;
    private int i;

    void Start()
    {
        instructions = new Queue<string>();
    }

    public void fillInstructions(InstructionsG dialogue)
    {
        instructions.Clear();

        // Fill Que with instructions
        foreach (string instruction in dialogue.instructions)
        {
            instructions.Enqueue(instruction);
        }
    }

    public void StartInstructions()
    {
 //       Debug.Log("Start " + dialogue.name);
 //       nameText.text = dialogue.name;

        // Clear instructions at the start
        instructions.Clear();

        // Populate Que with instructions
        fillInstructions(null);

        // Total Number of instructions in Que
        i = totalInstructions = instructions.Count;
        Debug.LogError(i);

        DisplayNextSentence();
    }

    

public void DisplayNextSentence()
    {
        if (instructions.Count == 0)
        {
        EndDialogue();
        return;
        }

        instruction = instructions.Dequeue();
        i--;
        Debug.Log(i);
        
        dialogueText.text = instruction;
    }

  public void DisplayPreviousSentence()
    {
        fillInstructions(null);

        if (i >= totalInstructions)
        {
            i = totalInstructions - 1;
            return;
        }

        instructionsLeft = totalInstructions - i;
               
        for (int j = 0; j < instructionsLeft; j++ )
        {
            instruction = instructions.Dequeue();
        }

        i++;
        Debug.Log(i);
        dialogueText.text = instruction;
    }
  

    void EndDialogue()
    {
        Debug.Log("End of Conversation");
    }
}
