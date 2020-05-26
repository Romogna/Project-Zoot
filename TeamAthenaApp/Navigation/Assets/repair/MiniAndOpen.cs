/***
 Program to Toggle the Repair Application from Minimized/ Open
 Date May 15th 2020
 Author Kory Menke
 Adds the keywords open and mini, and functions that tell what those keywords do, or with the press of the button
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.WSA.WebCam;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Windows.Speech;


public class MiniAndOpen : MonoBehaviour
{
    public GameObject Begin;
    public GameObject Previous;
    public GameObject Photo;
    public GameObject Next;
    public GameObject DialogueBox;
    public GameObject QRScan;
    public GameObject AudioNote;
    public GameObject Navigate;
    public Text btext;
    // Start is called before the first frame update
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    void Start()
    {

        // Global Voice Command
        keywords.Add("Minimize", () =>
        {
            // Call the changeTarget function
            Debug.Log("Minimize");
            OpenS();

        });
        keywords.Add("Open", () =>
        {
            // Call the changeTarget function
            Debug.Log("Open");
            OpenS();

        });


        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();



    }
    // Voice commands function
    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    public void OpenS()
    {
        if (Begin.activeSelf)
        {
            btext.text = "Open";
            Begin.SetActive(false);
            Previous.SetActive(false);
            Photo.SetActive(false);
            QRScan.SetActive(false);
            Next.SetActive(false);
            DialogueBox.SetActive(false);
            AudioNote.SetActive(false);
            Navigate.SetActive(false);
        }
        else
        {
            btext.text = "Minimize";
            Begin.SetActive(true);
            Previous.SetActive(true);
            Photo.SetActive(true);
            QRScan.SetActive(true);
            Next.SetActive(true);
            DialogueBox.SetActive(true);
            AudioNote.SetActive(true);
            Navigate.SetActive(true);
        }
    }
}
