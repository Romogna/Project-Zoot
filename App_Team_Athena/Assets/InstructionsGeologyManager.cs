using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class InstructionsGeologyManager : MonoBehaviour
{
//    KeywordRecognizer keywordRecognizer;
//    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    public Text nameText;
    public Text dialogueText;
    public string instruction;

    private ArrayList instructions;
    private int totalInstructions;
    private int instructionNumber;

    void Start()
    {
        //Create keywords for keyword recognizer
 //       keywords.Add("activate", () =>
  //      {
  //          // action to be performed when this keyword is spoken
  //      });

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
        instructionNumber = -1;

        // Call DisplayNextSentence method
        DisplayNextSentence();
    }
        
public void DisplayNextSentence()
    {
        // Increment count by 1
        instructionNumber++;
        
        // Check if count is greater than array size. If greater, decrement count.
        if (instructions.Count <= instructionNumber)
        {
            instructionNumber = instructionNumber - 1;
        }

        Debug.Log(instructionNumber);

        // store instruction and change array object to an array string.
        instruction = (string)instructions[instructionNumber];
                       
        // Display the instruction
        dialogueText.text = instruction;
    }

  public void DisplayPreviousSentence()
    {
        // Decrement count
        instructionNumber--;
        
        // Check if count drops below 0. If it does, set it back to 0.
        if (instructionNumber < 0)
        {
            instructionNumber = 0;
        }

        Debug.Log(instructionNumber);

        // store instruction and change array object to an array string.
        instruction = (string)instructions[instructionNumber];

        // Display the instruction
        dialogueText.text = instruction;
    }
  

    void EndDialogue()
    {
        Debug.Log("End of Conversation");
    }
}
