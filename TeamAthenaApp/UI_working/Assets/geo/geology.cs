﻿/***
 Basic repair btn control
 Date 2020
 Author Gw,Richard
 Voice Cited from API from Kam ,Romogna and Kory
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.WSA.WebCam;
using UnityEngine.UI;
using System.IO;
//voice code from Kam and Romogna
using UnityEngine.Windows.Speech;

public class geology : MonoBehaviour
{
    public GameObject pan;
    public Text btext;
    // Start is called before the first frame update
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    void Start()
    {

        // Global Voice Command
        keywords.Add("Geology", () =>
        {
            // Call the changeTarget function
            Debug.Log("Geology");
            OpenS();

        });
       /* keywords.Add("Done", () =>
        {
            // Call the changeTarget function
            Debug.Log("done");
            OpenS();

        });*/


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
        if (pan.activeSelf)
        {
            btext.text = "Geology";
            pan.SetActive(false);
        }
        else
        {
            btext.text = "Geology X";
            pan.SetActive(true);
        }
    }
}
