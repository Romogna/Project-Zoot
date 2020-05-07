//voice from Initial Navigation App
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Voice
using UnityEngine.Windows.Speech;
using System.Linq;

public class Main_butn : MonoBehaviour
{
    public GameObject pan;
    //public GameObject warn_pan;
    //public GameObject e_pan;
    public GameObject super_but;
    public Text btext;
    public Text b1text;
    //Text txt = transform.Find("Text").GetComponent<Text>();
    // Speech
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    float timeLeft = 30.0f;
    //int a = 0;
    // Use this for using keywords to activate functions
    void Start()
    {



        //b1text.text = "Sign";
        // Global Voice Command
        keywords.Add("mini", () =>
        {
            // Call the changeTarget function

            Debug.Log("Mini");
            Mini();

        });

        keywords.Add("Display", () =>
        {
            // Call the changeTarget function

            Debug.Log("Diaplay");
            Display();

        });


        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }
    void Update()
    {

        if (timeLeft >= -4)
        {
            timeLeft -= Time.deltaTime;
            Debug.Log(timeLeft);
            if (timeLeft < 15)
            {
                Debug.Log("warning");
                //warn_pan.SetActive(true);
                super_but.GetComponent<Image>().color = Color.yellow;


            }
            if (timeLeft < 5)
            {
                Debug.Log("emergency");
                //warn_pan.SetActive(false);
                //e_pan.SetActive(true);
                super_but.GetComponent<Image>().color = Color.red;
            }
            if (timeLeft < -3)
            {
                //e_pan.SetActive(false);
                super_but.GetComponent<Image>().color = Color.green;
            }
        }

    }
    public void Change()
    {

        Text txt = transform.Find("Text").GetComponent<Text>();

        if (pan.activeSelf)
        {
            txt.text = "Display";
            pan.SetActive(false);

        }
        else
        {
            txt.text = "Mini";
            pan.SetActive(true);


        }



    }

    public void Display()
    {
        pan.SetActive(true);
        btext.text = "Mini";

    }

    public void Mini()
    {
        pan.SetActive(false);
        btext.text = "Display";
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
}
