using System;
using System.Text;                          // needed for Stringbuilder
using System.Collections;
using System.Collections.Generic;
using System.Linq;                          // needed for voice keyword recognizer
using UnityEditor;                          // needed for voice dictation recognizer
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;           // needed for voice keyword & dictation recognizer
using TMPro;                                // needed for text mesh pro

public class InstructionsGeologyManager : MonoBehaviour
{

    // Set up dictation recognition
    private DictationRecognizer dictationRecognizer;

    // used to indicate whether dictation is on or off
    public TextMeshProUGUI dictationText;

    // private variables for dictation recognition
    [SerializeField]
    private TextMeshProUGUI dictationDisplay;

    // Use this to reset the UI once the Microphone is done recording after it was started.
    private bool hasRecordingStarted;

    // Set up phrase recognition
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
     
    public bool IsRunning;


    // Start is called before the first frame update
    void Start()
    {        
        keywords.Add("begin note taking", () =>
        { noteTaking(); /* calls the note taking method */});

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;

        Debug.Log("Initiliazed keyword recognizer");

        // Start keyword rcognizer
        // Note: keyword recognizer needs to be called immediately after setting up keyword parameters
        keywordRecognizer.Start();

        Debug.Log("Keyword recognizer started");

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
      

    /// <summary> ############################################################
    /// ---------------------  Dictation Methods below  ----------------------
    /// </summary> ###########################################################
    public void noteTaking()
    {
        if (!IsRunning)
        {
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

            // Used for debugging to show dictation parameters has been activated.
            // So, dictation can be used in App.
            Debug.Log("Initiliazed Dictation Recognizer");

            // Start dictation recogntion
            dictationRecognizer.Start();

            // Change bool to true for dictation control
            IsRunning = true;

            checkDictationOn();

            // Used for debugging to show dictation recognizer has started.
            Debug.Log("Dictation started");
        }
    }

    private void dictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        // Displays what the App belives was spoken and displays it in console
        Debug.LogFormat("Dictation result: {0}", text);

        // Displays what was said to the UI
        dictationDisplay.text = text;
    }

    void dictationRecognizer_DictationHypothesis(string text)
    {
        // Displays what the App processed was spoken
        Debug.LogFormat("Dictation hypothesis: {0}", text);
    }

    void dictationRecognizer_DictationComplete(DictationCompletionCause completionCause)
    {
        if (completionCause != DictationCompletionCause.Complete)
        {
            Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
        }
        Debug.Log("Dictation complete");

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
        IsRunning = false;
        Debug.Log("IsRunning is " + IsRunning);
        checkDictationOn();
        Debug.Log("IsRunning is " + IsRunning);
        PhraseRecognitionSystem.Restart();
        Debug.Log("IsRunning is " + IsRunning);
        keywordRecognizer.Start();
        
    }

    public void checkDictationOn()
    {
        if (IsRunning)
        {
            dictationListening();
        }
        else
            dictationOff();
    }

    private void dictationListening()
    {
        Debug.Log("Dictation Listening On");
        this.dictationText.text = "Dictation ON";
    }

    private void dictationOff()
    {
        Debug.Log("Dictation Off");
        this.dictationText.text = "";
    }
      
}


