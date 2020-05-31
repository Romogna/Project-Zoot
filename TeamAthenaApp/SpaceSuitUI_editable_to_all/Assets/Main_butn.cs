/**
 * File:Main_butn.cs
 * Date:05/2020
 * By Richard and guan 
 * suitvital's code
 * vioce code from Kam and Emery Initial Navigation App
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
    public GameObject color_bt;
    public GameObject pan;
    public GameObject warn_pan;
    public GameObject e_pan;
    public GameObject super_but;
    public Text btext;
    public Text b1text;
    Teleinfo info;
    public Text counttext;
    public Text time_text;
    public Text bat_text;
    public Text o2_text;
    public Text h20_text;
    public Text temp_text;
    public Text subp_text;
    public Text heart_text;
    public Text fan_text;
    //Text txt = transform.Find("Text").GetComponent<Text>();
    // Speech
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    /*
     * float timeLeft = 100.0f;
    //for local test
    float battimeLeft = 4*60;
    float o2timeLeft = 6*60;
    float h20imeLeft = 6*60;
    float evatime = 0;
    */
    //display text in the panel
    //[SerializeField] Text counttext;
    [System.Serializable]
    public class Teleinfo
    {
        public string _id;
        public double time;
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
        /*keywords.Add("mini", () =>
        {
            // Call the changeTarget function
            Debug.Log("Mini");
            Mini();

        });*/

        keywords.Add("Vital", () =>
        {
            // Call the changeTarget function
            Debug.Log("Vital");
            Change();

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
        //StartCoroutine(GetRequest("http://gooabcgle.com"));
       StartCoroutine(GetRequest("http://45.37.165.34:3000/api/simulation/state"));
       
        //from file
        //string path = string.Format("{0}/mydata/{1}.json", Application.persistentDataPath, filename);
        /*
        o2timeLeft -= Time.deltaTime;
        h20imeLeft -= Time.deltaTime;
        battimeLeft -= Time.deltaTime;
        evatime += Time.deltaTime;
        
        //Application.dataPath
        //Application.persistentDataPath
        //string tele_in = File.ReadAllText(Application.persistentDataPath + "/Test_1.json");
        //info = JsonUtility.FromJson<Teleinfo>(tele_in);
        //Debug.Log(info.timer.ToString());
        
        Teleinfo t = new Teleinfo();
        t._id = "5eb45106a2e9745e38d1d8c6";
        t.time = 8525.543000000038;
        t.timer="02:22:05";
        t.started_at= "2020-05-07T18:18:46.191Z";
        t.heart_bpm=90;
        t.p_sub="3.93";
        t.p_suit= "3.95";
        t.t_sub= "31.7";
        t.v_fan= "39986";
        t.p_o2= "775.02";
        t.rate_o2= "0.6";
        t.batteryPercent=40.79484027777724;
        t.battery_out = 40;
        t.cap_battery = 29;
        t.t_battery="01:37:54";
        t.p_h2o_g= "15.18";
        t.p_h2o_l= "15.89";
        t.p_sop="882";
        t.rate_sop= "1.0";
        t.t_oxygenPrimary = "21.05978703703501";
        t.t_oxygenSec="100";
        t.ox_primary= "21";
        t.ox_secondary= "100";
        t.t_oxygen="03:37:54";
        t.cap_water= 56.94170202020222;
        t.t_water="03:07:54";
        t.__v= 0;
        //counttext.text = tele_in;
        string tjson = JsonUtility.ToJson(t);
        Debug.Log(tjson.ToString());
        counttext.text = tjson.ToString();
        
       // time_text.text="EVA time: "+t.timer.ToString();
       // time_text.text = "EVA time:     " + System.TimeSpan.FromSeconds(evatime).ToString();
        //bat_text.text = "Battery left: " + t.t_battery.ToString();
        bat_text.text =  "Battery left: " + System.TimeSpan.FromSeconds(battimeLeft).ToString();
        //o2_text.text = "02 left: " + t.t_oxygen.ToString();
        o2_text.text =   "02 left:       " + System.TimeSpan.FromSeconds(o2timeLeft).ToString();
        // h20_text.text = "H20 left: " + t.t_oxygen.ToString();
        h20_text.text =  "H20 left:      " + System.TimeSpan.FromSeconds(h20imeLeft).ToString();
        temp_text.text = "Temp:" + t.t_sub+"F";
        subp_text.text = "Press:" + t.p_sub+"psia";
        heart_text.text = "Heart:" + t.heart_bpm+"bmp";
        fan_text.text = "Fan:" + t.v_fan + "rpm";
        if (t.heart_bpm < 60 || t.heart_bpm > 100 || System.Convert.ToInt32(t.v_fan)<10000)
        {

            Debug.Log("Emergency");

            super_but.GetComponent<Image>().color = Color.red;
            warn_pan.SetActive(false);
            e_pan.SetActive(true);


        }
        if (timeLeft >= -4)
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
        }
        */
    }
    public void Change()
    {

        Text txt = transform.Find("Text").GetComponent<Text>();
        if (pan.activeSelf)
        {
            txt.text = "Vital";
            pan.SetActive(false);
        }
        else
        {
            txt.text = "Vital X";
            pan.SetActive(true);
        }
    }

    public void Display()
    {
        pan.SetActive(true);
        btext.text = "Vital X";
    }

    public void Mini()
    {
        pan.SetActive(false);
        btext.text = "Vital";
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
                if (webRequest.downloadHandler.text==null) {

                    Debug.Log("No connection");
                }
                info =JsonUtility.FromJson<Teleinfo>(webRequest.downloadHandler.text);
                //Debug.Log(info.timer.ToString());
                time_text.text = "EVA time:     " + info.timer.ToString().ToString();
                bat_text.text = "Battery left: " +info.t_battery.ToString();
                o2_text.text = "02 left:       " + info.t_oxygen.ToString();
                h20_text.text = "H20 left:      " + info.t_water.ToString();
                temp_text.text = "Temp:" + info.t_sub + "F";
                subp_text.text = "Press:" + info.p_sub + "psia";
                heart_text.text = "Heart:" + info.heart_bpm + "bmp";
                fan_text.text = "Fan:" + info.v_fan + "rpm";
                if(System.Convert.ToInt32(info.v_fan) < 10000)
                {

                    Debug.Log("Emergency");
                    
                    super_but.GetComponent<Image>().color = Color.red;
                    //color_bt.GetComponent<Image>().color= Color.red;
                    warn_pan.SetActive(false);
                    e_pan.SetActive(true);


                }
                else if (System.Convert.ToInt32(info.v_fan) < 15000 || System.Convert.ToInt32(info.v_fan)> 40000)
                {

                   
                    Debug.Log("Warning");

                    super_but.GetComponent<Image>().color = Color.yellow;
                    //color_bt.GetComponent<Image>().color = Color.yellow;
                    warn_pan.SetActive(true);
                    e_pan.SetActive(false);

                }
                else { 

                    super_but.GetComponent<Image>().color = Color.green;
                    //Color c = new Color32(19, 210, 241, 255);
                    //color_bt.GetComponent<Image>().color = Color.blue;
                    warn_pan.SetActive(false);
                    e_pan.SetActive(false);
                }

            }
        }
    }
}
