using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech; //keyword recognizer
using System.Linq;//keyword recognizer

public class DialogueTrigger : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;//keyword recognizer
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();//keyword recognizer

    public Dialogue dialogue;
    void Start()
    {
        //Create keywords for keyword recognizer
        keywords.Add("start", () =>
        {
            // action to be performed when this keyword is spoken
            TriggerDialogue();
        });

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;

        keywordRecognizer.Start(); 

    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
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
