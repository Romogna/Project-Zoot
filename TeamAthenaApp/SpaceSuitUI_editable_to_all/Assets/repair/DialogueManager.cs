using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech; //keyword recognizer
using System.Linq;//keyword recognizer

public class DialogueManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;//keyword recognizer
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();//keyword recognizer

    // Bool which app is currently running
    public bool tireAppRunning;

    // Used to display text to display
    public Text dialogueText;
    public Text geoDialogueText;

    // -------------- Setup for Tire Instructions --------------------
    public Dialogue dialogue;
    private string sentence; // Emery code but changed instruction to sentence
    private ArrayList sentences; // Emery code but changed instructions to sentences
    private int totalInstructions; // Emery code
    private int instructionNumber; // Emery code

    // -------------- Setup for Geology Instructions ------------------
    public Geology geology;
    public string instruction;
    private ArrayList instructions;
    private int totalGeoInstructions;
    private int geoInstructionNumber;

    // -------------- Setup for Dictation Recognizer ------------------
    private DictationRecognizer dictationRecognizer;
    [SerializeField]
    private Text dictationDisplay;
    private StringBuilder textSoFar;
    public bool isRunning;
    public Text dictationIndicatorText;

    // -------------- Setup to save Dictation --------------------------
    // Path to file
    public string path;
    public string content;

    void Start()
    {
        //Create keywords for keyword recognizer
        keywords.Add("start geology", () =>
        {
            tireAppRunning = false;
            TriggerGeologyInstructions();
        });
        keywords.Add("start repair", () =>
        {
            tireAppRunning = true;
            TriggerTireInstructions();
        });
        keywords.Add("next", () =>
        {
           DisplayNextSentence();
        });
        keywords.Add("previous", () =>
        {
           DisplayPreviousSentence();
        });
        keywords.Add("begin note", () =>
        {
            noteTaking();
        });

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;

        keywordRecognizer.Start();
        
        // For Tire App instructions
        sentences = new ArrayList();

        // For Geology App instructions
        instructions = new ArrayList();
    }

    /// <summary>################################################################
    ///  ---------------------- Tire Instructions -------------------------------
    /// </summary>###############################################################
    public void TriggerTireInstructions()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void StartDialogue (Dialogue dialogue)
    {
        tireAppRunning = true;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
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
    
    /// <summary>################################################################
    ///  --------------------- Geolgy Instructions ------------------------------
    /// </summary>###############################################################
    public void TriggerGeologyInstructions()
    {
        FindObjectOfType<DialogueManager>().StartGeology(geology);
    }

    public void StartGeology(Geology geology)
    {
        tireAppRunning = false;

        instructions.Clear();

        foreach (string instruction in geology.instructions)
        {
            instructions.Add(instruction);
        }
        // used for debugging
        totalGeoInstructions = instructions.Count;
        Debug.Log(totalGeoInstructions);

        // Initialize code count to -1. Because count is set to 0 in DisplayNextSentence method.
        geoInstructionNumber = -1;

        DisplayNextSentence();
    }

    /// <summary>################################################################
    ///  --------------------- Instructions Control -----------------------------
    /// </summary>###############################################################

    public void DisplayNextSentence()
    {
        Debug.Log("Tire is bool is " + tireAppRunning);

        if (tireAppRunning)
        {
            // Increment count by 1
            instructionNumber++;

            // Check if count is greater than array size. If greater, decrement count.
            if (sentences.Count <= instructionNumber)
            {
                instructionNumber = instructionNumber - 1;
            }
            Debug.Log("Tire " + instructionNumber);

            // store instruction and change array object to an array string.
            sentence = (string)sentences[instructionNumber];

            // Display the instruction
            dialogueText.text = sentence;
        }

        if (!tireAppRunning)
        {
            // Increment count by 1
            geoInstructionNumber++;

            if (instructions.Count <= geoInstructionNumber)
            {
                geoInstructionNumber = geoInstructionNumber - 1;
            }

            // script is for debugging current instruction number
            Debug.Log("Geology " + geoInstructionNumber);

            // store instruction and change array object to an array string.
            instruction = (string)instructions[geoInstructionNumber];

            // Display the instruction
            geoDialogueText.text = instruction;
        }
    }

    public void DisplayPreviousSentence()
    {
        Debug.Log("Tire app is " + tireAppRunning);

        if (tireAppRunning)
        {
            // Decrement count
            instructionNumber--;

            // Check if count drops below 0. If it does, set it back to 0.
            if (instructionNumber < 0)
            {
                instructionNumber = 0;
            }

            // script is for debugging current instruction number
            Debug.Log("Tire " + instructionNumber);

            // store instruction and change array object to an array string.
            sentence = (string)sentences[instructionNumber];

            // Display the instruction
            dialogueText.text = sentence;
        }

        if (!tireAppRunning)
        {
            Debug.Log("In Geology Instructions");

            // Decrement count
            geoInstructionNumber--;

            // Check if count drops below 0. If it does, set it back to 0.
            if (geoInstructionNumber < 0)
            {
                geoInstructionNumber = 0;
            }

            // script is for debugging current instruction number
            Debug.Log("Geology " + geoInstructionNumber);

            // store instruction and change array object to an array string.
            instruction = (string)instructions[geoInstructionNumber];

            // Display the instruction
            geoDialogueText.text = instruction;
        }
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

/// <summary> ############################################################
/// ---------------------  Dictation Methods below  ----------------------
/// </summary> ###########################################################
    public void noteTaking()
    {
        if (!isRunning)
        {
            // Create file to store dictation text
            CreateText();

            // stop keyword recognizer to prevent dictation recognition conflict
            PhraseRecognitionSystem.Shutdown();

            Debug.Log("Shutting down Phrase Recognition");

            dictationRecognizer = new DictationRecognizer();

            dictationRecognizer.InitialSilenceTimeoutSeconds = 6f;
            dictationRecognizer.AutoSilenceTimeoutSeconds = 6f;

            dictationRecognizer.DictationResult += dictationRecognizer_DictationResult;
            dictationRecognizer.DictationHypothesis += dictationRecognizer_DictationHypothesis;
            dictationRecognizer.DictationComplete += dictationRecognizer_DictationComplete;
            dictationRecognizer.DictationError += dictationRecognizer_DictationError;

            textSoFar = new StringBuilder();

            // Used for debugging to show dictation parameters has been activated.
            // So, dictation can be used in App.
            Debug.Log("Initiliazed Dictation Recognizer");

            // Start dictation recogntion
            dictationRecognizer.Start();

            // Change bool to true for dictation control
            isRunning = true;

            checkDictationOn();

            // Used for debugging to show dictation recognizer has started.
            Debug.Log("Dictation started");
        }
    }

    private void dictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        // Displays what the App belives was spoken and displays it in console
        Debug.LogFormat("Dictation result: {0}", text);

        textSoFar.Append(text + ". \n");

        // Displays what was said to the UI
        dictationDisplay.text = textSoFar.ToString();

        content = "Login date: " + System.DateTime.Now + "\n" + textSoFar.ToString() + "\n";

        File.AppendAllText(path, content);
    }

    void dictationRecognizer_DictationHypothesis(string text)
    {
        // Displays what the App processed was spoken
        Debug.LogFormat("Dictation hypothesis: {0}", text);

        dictationDisplay.text = textSoFar.ToString() + " " + text + "...";
    }

    void dictationRecognizer_DictationComplete(DictationCompletionCause completionCause)
    {
        if (completionCause != DictationCompletionCause.Complete)
        {
            Debug.Log("Dictation complete");
        }

        dictationDisplay.text = "";

        keywordRestart();
    }

    void dictationRecognizer_DictationError(string error, int hresult)
    {
        Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
    }

    public void keywordRestart()
    {
        dictationRecognizer.Stop();
        dictationRecognizer.Dispose();
        isRunning = false;
        checkDictationOn();
        PhraseRecognitionSystem.Restart();
        keywordRecognizer.Start();
    }

    public void checkDictationOn()
    {
        if (isRunning)
        {
            dictationListening();
        }
        else
            dictationOff();
    }

    private void dictationListening()
    {
        Debug.Log("Dictation Listening On");
        this.dictationIndicatorText.text = "Dictation ON";
    }

    private void dictationOff()
    {
        Debug.Log("Dictation Off");
        this.dictationIndicatorText.text = "Dictation OFF";
    }


    public void CreateText()
    {
        string set = Application.persistentDataPath;
        if (tireAppRunning)
        {

            path = set + "/00_Tire_Repair_Notes.txt";
           
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "Tire_repair_notes \n\n");
            }

            Debug.Log("Save to Tire Repair Notes");
        }

        if (!tireAppRunning)
        {

            path = set + "/00_Geology_Notes.txt";

            if (!File.Exists(path))
            {
                File.WriteAllText(path, "Geology notes \n\n");
            }

            Debug.Log("Save to Geology Notes");
        }
    }
}
