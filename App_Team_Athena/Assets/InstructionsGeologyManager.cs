using System;
using System.Text;                          // needed for Stringbuilder
using System.Collections;
using System.Collections.Generic;
using System.Linq;                          // needed for voice keyword recognizer
using UnityEditor;                          // needed for voice dictation recognizer
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;           // needed for voice keyword & dictation recognizer

public class InstructionsGeologyManager : MonoBehaviour
{
// Set up dictation recognition
    private DictationRecognizer dictationRecognizer;

    // public variables for dictation recognition
    // Use this string to cache the text currently displayed in the text box.
    //private StringBuilder textSoFar;
    public Text dictationText;

    // Using an empty string specifies the default microphone.
    /*    private static string deviceName = string.Empty;
        private int samplingRate;
        private const int messageLength = 10;
    */

    // private variables for dictation recognition
    [SerializeField]
    private Text dictationDisplay;

    // Use this to reset the UI once the Microphone is done recording after it was started.
    private bool hasRecordingStarted;

// Set up phrase recognition
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

// public variables for instructions
    public InstructionsG dialogue;
    public Text nameText;
    public Text dialogueText;
    public string instruction;
    public bool IsRunning;

    // private variables for instructions 
    private ArrayList instructions;
    private int totalInstructions;
    private int instructionNumber;

// Initialize dictation recognizer here, but does not start it
    private void Awake()
    {
        
    }
 
// Initialize and start keyword recognition here    
    void Start()
    {
        //Create keywords for keyword recognizer
        keywords.Add("next page", () =>
        { DisplayNextSentence();  /* calls the DisplayNextSentence method */});

        keywords.Add("previous page", () =>
        { DisplayPreviousSentence();  /* calls the DisplayPreviousSentence method */});

        keywords.Add("start over", () =>
        { pageOne();  /* calls the page 1 method */});

        keywords.Add("go to page 2", () =>
        { pageTwo();  /* calls the page two method */});

        keywords.Add("go to page 3", () =>
        { pageThree();  /* calls the page three method */});

        keywords.Add("go to page 4", () =>
        { pageFour();  /* calls the page four method */});

        keywords.Add("go to page 5", () =>
        { pageFive();  /* calls the page five method */});

        keywords.Add("go to page 6", () =>
        { pageSix();  /* calls the page six method */});
        
        keywords.Add("go to page 7", () =>
        { pageSeven();  /* calls the pageseven method */});

        keywords.Add("go to page 8", () =>
        { pageEight();  /* calls the pageeight method */});

        keywords.Add("last page", () =>
        { lastPage();  /* calls the lastpage method */});

        keywords.Add("begin note taking", () =>
        { noteTaking(); /* calls the note taking method */});

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;

        Debug.Log("Initiliazed keyword recognizer");

        // Start keyword rcognizer
        // Note: keyword recognizer needs to be called immediately after setting up keyword parameters
        keywordRecognizer.Start();
               
        Debug.Log("Keyword recognizer started");

        // Start an array of unknown size
        instructions = new ArrayList();
              
    }

// Start instruction control functions here          
    public void StartInstructions(InstructionsG dialogue)
    {
     //       Debug.Log("Start " + dialogue.name);
     //       nameText.text = dialogue.name;

        // Clear array of previous instructions at the start
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

// Start Instruction controls here
        public void DisplayNextSentence()
        {
            // Increment count by 1
            instructionNumber++;
        
            // Check if count is greater than array size. If greater, decrement count.
            if (instructions.Count <= instructionNumber)
            {
                instructionNumber = instructionNumber - 1;
            }

            // script is for debugging current instruction number
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

            // script is for debugging current instruction number
            Debug.Log(instructionNumber);

            // store instruction and change array object to an array string.
            instruction = (string)instructions[instructionNumber];

            // Display the instruction
            dialogueText.text = instruction;
        }

        public void pageOne()
        {
            // Set instruction page number to 0
            instructionNumber = 0;

            // store instruction and change array object to an array string.
            instruction = (string)instructions[instructionNumber];

            // Display the instruction
            dialogueText.text = instruction;
        }

        public void pageTwo()
        {
            // Set instruction page number to 1
            instructionNumber = 1;

            // store instruction and change array object to an array string.
            instruction = (string)instructions[instructionNumber];

            // Display the instruction
            dialogueText.text = instruction;
        }

        public void pageThree()
        {
            // Set instruction page number to 2
            instructionNumber = 2;

            // store instruction and change array object to an array string.
            instruction = (string)instructions[instructionNumber];

            // Display the instruction
            dialogueText.text = instruction;
        }

        public void pageFour()
        {
            // Set instruction page number to 3
            instructionNumber = 3;

            // store instruction and change array object to an array string.
            instruction = (string)instructions[instructionNumber];

            // Display the instruction
            dialogueText.text = instruction;
        }

        public void pageFive()
        {
            // Set instruction page number to 4
            instructionNumber = 4;

            // store instruction and change array object to an array string.
            instruction = (string)instructions[instructionNumber];

            // Display the instruction
            dialogueText.text = instruction;
        }

        public void pageSix()
        {
            // Set instruction page number to 5
            instructionNumber = 5;

            // store instruction and change array object to an array string.
            instruction = (string)instructions[instructionNumber];

            // Display the instruction
            dialogueText.text = instruction;
        }

        public void pageSeven()
        {
            // Set instruction page number to 6
            instructionNumber = 6;

            // store instruction and change array object to an array string.
            instruction = (string)instructions[instructionNumber];

            // Display the instruction
            dialogueText.text = instruction;
        }

        public void pageEight()
        {
            // Set instruction page number to 7
            instructionNumber = 7;

            // store instruction and change array object to an array string.
            instruction = (string)instructions[instructionNumber];

            // Display the instruction
            dialogueText.text = instruction;
        }

        public void lastPage()
        {
            // Set instruction page number to 8
            instructionNumber = 8;

            // store instruction and change array object to an array string.
            instruction = (string)instructions[instructionNumber];

            // Display the instruction
            dialogueText.text = instruction;
        }
    
// Start Phrase Recognition funtion here
        private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            System.Action keywordAction;
            // if the keyword recognized is in our dictionary, call that Action.
            if (keywords.TryGetValue(args.text, out keywordAction))
            {
                keywordAction.Invoke();
            }
        }



    // Start Dictation Recognition functions here

  
        




/*        private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
        {
            Debug.Log("Adding text");
            dictationDisplay.text += text + "\n";
    }


        public void DictationRecognizer_DictationHypothesis(string text)
        {
            Debug.Log("Dictation Hypothesis working");
            dictationDisplay.text = textSoFar.ToString() + " " + text + "...";
        }

        public void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
        {
        Debug.Log("Dictation complete");
            if (cause == DictationCompletionCause.TimeoutExceeded)
            {
                dictationDisplay.text = "Dictation Off";
            }
        }
    
        public void DictationRecognizer_DictationError(string error, int hresult)
        {
            dictationDisplay.text = error + "\nHRESULT" + hresult;
        }
*/

        public void noteTaking()
        {

            // stop keyword recognizer to prevent dictation recognition conflict
            PhraseRecognitionSystem.Shutdown();

            Debug.Log("Shutting down Phrase Recognition");

            dictationRecognizer = new DictationRecognizer();

            dictationRecognizer.DictationResult += (string text, ConfidenceLevel confidence) =>
            {
                // Displays what the App belives was spoken and displays it in console
                Debug.LogFormat("Dictation result: {0}", text);

                // Displays what was said to the UI
                dictationDisplay.text = text;
            };

            dictationRecognizer.DictationHypothesis += (text) =>
            {
                // Displays what the App processed was spoken
                Debug.LogFormat("Dictation hypothesis: {0}", text);                                
            };

            dictationRecognizer.DictationComplete += (completionCause) =>
            {
                if (completionCause != DictationCompletionCause.Complete)
                    Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
                Debug.Log("Dictation complete");
            };

            dictationRecognizer.DictationError += (error, hresult) =>
            {
                Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
            };

            // Used for debugging to show dictation parameters has been activated.
            // So, dictation can be used in App.
            Debug.Log("Initiliazed Dictation Recognizer");
               
            // Start dictation recogntion
            dictationRecognizer.Start();

            // Change bool to true for dictation control
            IsRunning = true;

            // Used for debugging to show dictation recognizer has started.
            Debug.Log("Start dictation");

        }
}

