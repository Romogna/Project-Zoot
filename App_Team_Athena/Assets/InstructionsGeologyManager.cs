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
    private int instructionsLeft;
    private int i;

    void Start()
    {
        instructions = new ArrayList();
    }

    public void StartInstructions(InstructionsG dialogue)
    {
 //       Debug.Log("Start " + dialogue.name);
 //       nameText.text = dialogue.name;

        // Clear instructions at the start
        instructions.Clear();

        // Fill Que with instructions
        foreach (string instruction in dialogue.instructions)
        {
            instructions.Add(instruction);
        }

        totalInstructions = instructions.Count;
        Debug.Log(totalInstructions);

        i = -1;

        DisplayNextSentence();
    }
        
public void DisplayNextSentence()
    {
        i++;
        Debug.Log(i);

        if (instructions.Count <= i)
        {
            EndDialogue();
            return;
        }
                
        instruction = (string)instructions[i];
                       
        dialogueText.text = instruction;
    }

  public void DisplayPreviousSentence()
    {
        i--;
        Debug.Log(i);
 
        if (i < 0)
        {
            i = 0;
        }

        instruction = (string)instructions[i];

        dialogueText.text = instruction;
    }
  

    void EndDialogue()
    {
        Debug.Log("End of Conversation");
    }
}
