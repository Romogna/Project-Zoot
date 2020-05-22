using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech; //keyword recognizer
using System.Linq;//keyword recognizer

public class DialogueManagerG : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;//keyword recognizer
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();//keyword recognizer

    public Text nameText;
    public Text dialogueText;
    public string sentence; // Emery code but changed instruction to sentence

    //private Queue<string> sentences;
    private ArrayList sentences; // Emery code but changed instructions to sentences
    private int totalInstructions; // Emery code
    private int instructionNumber; // Emery code

    void Start()
    {

        //Create keywords for keyword recognizer
        keywords.Add("next", () =>
        {
            // action to be performed when this keyword is spoken
            DisplayNextSentence();
        });
        //Create keywords for keyword recognizer
        keywords.Add("previous", () =>
        {
            // action to be performed when this keyword is spoken
            DisplayPreviousSentence();
        });

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;

        keywordRecognizer.Start();


        sentences = new ArrayList();

    }

    public void StartDialogue(DialogueG dialogueG)
    {
        //nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogueG.sentences)
        {

            sentences.Add(sentence);
        }
        // used for debugging
        totalInstructions = sentences.Count;
        Debug.Log(totalInstructions);

        // Initialize code count to -1. Because count is set to 0 in DisplayNextSentence method.
        instructionNumber = -1;

        DisplayNextSentence();
    }


    public void DisplayNextSentence()
    {
        // Increment count by 1
        instructionNumber++;

        // Check if count is greater than array size. If greater, decrement count.
        if (sentences.Count <= instructionNumber)
        {
            instructionNumber = instructionNumber - 1;
        }

        // script is for debugging current instruction number
        Debug.Log(instructionNumber);

        // store instruction and change array object to an array string.
        sentence = (string)sentences[instructionNumber];

        // Display the instruction
        dialogueText.text = sentence;
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

        // script is for debugging current instruction number
        Debug.Log(instructionNumber);

        // store instruction and change array object to an array string.
        sentence = (string)sentences[instructionNumber];

        // Display the instruction
        dialogueText.text = sentence;
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        // if the keyword recognized is in our dictionary, call that Action.
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation.");
    }
}