/**
 * File:Main_butn,cs
 * Date:05/2020
 * suitvital's code
 * vioce code from Kam and Romogna Initial Navigation App
 * It will accept voice cpmmand or clicking display button to show
 * Voice command : "Display"<->"Mini"
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//voice code from Kam and Romogna
using UnityEngine.Windows.Speech;
using System.Linq;
using UnityEngine.Networking;
using System.IO;


public class Main_butn : MonoBehaviour
{
    public GameObject pan;
    public GameObject warn_pan;
    public GameObject e_pan;
    public GameObject super_but;
    public Text btext;
    public Text b1text;
    Teleinfo info;
    public Text counttext;
    //Text txt = transform.Find("Text").GetComponent<Text>();
    // Speech
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    //float timeLeft = 30.0f;
   
    //display text in the panel
    //[SerializeField] Text counttext;
    [System.Serializable]
    public class Teleinfo
    {
        public string _id;
        public int time;
        public string timer;
        public string started_at;
        public int heart_bpm;
        //2-4 psid
        public string p_sub;
        //2-4 psid
        public string p_suit;
        //standard
        public string t_sub;
        //10000-40000 rpm
        public string v_fan;
        //750-950
        public string p_o2;
        //.5-1
        public string rate_o2;
        
        public double batteryPercent;
        public int battery_out;
        //0-30
        public int cap_battery;
        //0-10 hrs
        public string t_battery;
        //14-16
        public string p_h2o_g;
        //14-16
        public string p_h2o_l;
        //750-950
        public string p_sop;
        //0.5-1
        public string rate_sop;
        public string t_oxygenPrimary;
        public string t_oxygenSec;
        public string ox_primary;	
        public string ox_secondary;
        //0-10
        public string t_oxygen;
        public double cap_water;
        //0-10 hrs
        public string t_water;
        public int __v;
    }

    // Use this for using keywords to activate functions
    void Start()
    {
        //b1text.text = "Sign";
        //connect to database
        //StartCoroutine(GetRequest("http://localhost:3000/api/simulation/state"));
        //TimeSpan ts = TimeSpan.Parse("10:20:30");
        //double totalSeconds = ts.TotalSeconds;
        
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
        //from database
        //StartCoroutine(GetRequest("http://localhost:3000/api/simulation/state"));
        //from file
        string tele_in = File.ReadAllText(Application.dataPath + "/Test_1.json");
        info = JsonUtility.FromJson<Teleinfo>(tele_in);
        Debug.Log(info.timer.ToString());
        counttext.text = tele_in;
        if (info.heart_bpm < 60 || info.heart_bpm > 100)
        {

            Debug.Log("Emergency");

            super_but.GetComponent<Image>().color = Color.red;
            warn_pan.SetActive(false);
            e_pan.SetActive(true);


        }
        /*if (timeLeft >= -4)
        {
            timeLeft -= Time.deltaTime;
            //Debug.Log(timeLeft);
            if (timeLeft < 24)
            {
                Debug.Log("warning");
                warn_pan.SetActive(true);
                super_but.GetComponent<Image>().color = Color.yellow;
            }
            if (timeLeft < 10)
            {
                Debug.Log("emergency");

                super_but.GetComponent<Image>().color = Color.red;
                warn_pan.SetActive(false);
                e_pan.SetActive(true);
            }
           
        }else {
            e_pan.SetActive(false);
            warn_pan.SetActive(false);
        }*/

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


    //get touch with database
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
                counttext.text =  webRequest.error;
            }
            else
            {
                //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                counttext.text = webRequest.downloadHandler.text;
                //JSONObject json = new JSONObject(www.text);
                string response = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                //Teleinfo player= JsonUtility.ToJson(response);
                //Teleinfo player = JsonConvert.DeserializeObject<Teleinfo>(response);
                // Teleinfo t = new Teleinfo();
                //Teleinfo json = new Teleinfo(webRequest.downloadHandler.text);
                info =JsonUtility.FromJson<Teleinfo>(webRequest.downloadHandler.text);
                Debug.Log(info.timer.ToString());
                /*if (info.heart_bpm < 60 || info.heart_bpm > 100)
                {

                    Debug.Log("Emergency");

                    super_but.GetComponent<Image>().color = Color.red;
                    warn_pan.SetActive(false);
                    e_pan.SetActive(true);


                }*/
            }
        }
    }
}
