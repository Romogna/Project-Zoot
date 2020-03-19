using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsGeologyManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public string instruction;

    private ArrayList instructions;
    private int totalInstructions;
    private int i;

    void Start()
    {
        // Start an array of unknown size
        instructions = new ArrayList();
    }

    public void StartInstructions(InstructionsG dialogue)
    {
 //       Debug.Log("Start " + dialogue.name);
 //       nameText.text = dialogue.name;

        // Clear instructions at the start
        instructions.Clear();

        // Fill Array with instructions and establish size
        foreach (string instruction in dialogue.instructions)
        {
            instructions.Add(instruction);
        }

        // used for debugging
        totalInstructions = instructions.Count;
        Debug.Log(totalInstructions);

        // Initialize code count to -1. Because count is set to 0 in DisplayNextSentence method.
        i = -1;

        // Call DisplayNextSentence method
        DisplayNextSentence();
    }
        
public void DisplayNextSentence()
    {
        // Increment count by 1
        i++;
        Debug.Log(i);

        // Check if count is greater than array size. If greater, decrement count.
        if (instructions.Count <= i)
        {
            i = i - 1;
        }
        
        // store instruction and change array object to an array string.
        instruction = (string)instructions[i];
                       
        // Display the instruction
        dialogueText.text = instruction;
    }

  public void DisplayPreviousSentence()
    {
        // Decrement count
        i--;
        Debug.Log(i);

        // Check if count drops below 0. If it does, set it back to 0.
        if (i < 0)
        {
            i = 0;
        }

        // store instruction and change array object to an array string.
        instruction = (string)instructions[i];

        // Display the instruction
        dialogueText.text = instruction;
    }
  

    void EndDialogue()
    {
        Debug.Log("End of Conversation");
    }
}
