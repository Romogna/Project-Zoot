using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
// Voice
using UnityEngine.Windows.Speech;
using System.Linq;
using System.Collections.Generic;

public class EventProcessor : MonoBehaviour
{
    // For testing
    public Text TextTime;
    public Text TextLatitude;
    public Text TextLongitude;
    public Text TextLumens;
    public Text GpsHeading;

    //public Image Renderer; // Used to display map
    public String message = "Distance: ";

    // Camera transform
    public Transform playerTransform;
    Vector3 direct;

    // GPS calculations
    double UserLat;
    double UserLong;
    double TargetLat;
    double TargetLong;
    double X_heading;
    double Y_heading;

    // Toggle Canvas
    public GameObject NaviMenu;

    // Toggle Compass
    public GameObject SpriteArrow;
    public GameObject HereSign;
    public Text TargetHeading;

    // Voice
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    private System.Object _queueLock = new System.Object();
    List<byte[]> _queuedData = new List<byte[]>();
    List<byte[]> _processingData = new List<byte[]>();

    public void QueueData(byte[] data)
    {
        lock (_queueLock)
        {
            _queuedData.Add(data);
        }
    }

    void Start()
    {

        // Global Voice Command
        keywords.Add("Rover", () =>
        {
            changeTarget(1);
        });
        keywords.Add("Home", () =>
        {
            changeTarget(2);
        });
        keywords.Add("Sampling Site", () =>
        {
            changeTarget(3);
        });
        keywords.Add("Navigation", () =>
        {
            togglePanel();
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();


        changeTarget(1);
    }

    void Update()
    {
        MoveQueuedEventsToExecuting();
        while (_processingData.Count > 0)
        {

            try
            {
                var byteData = _processingData[0];
                _processingData.RemoveAt(0);

                var gpsData = GPS_DataPacket.ParseDataPacket(byteData);
                TextTime.text       = "     Time: " + DateTime.Now.ToString("t");
                TextLatitude.text   = " Latitude: " + gpsData.Latitude.ToString();
                TextLongitude.text  = "Longitude: " + gpsData.Longitude.ToString();
                TextLumens.text     = "   Lumens: " + gpsData.Lumens.ToString();
                GpsHeading.text     = "  Heading: " + gpsData.Heading.ToString();

                UserLat = gpsData.Latitude;      // converts to float
                UserLong = gpsData.Longitude;    // converts to float

                /*
                if (Renderer != null)
                {   // Not used
                    var url = "http://maps.googleapis.com/maps/api/staticmap?center=" + gpsData.Latitude.ToString("F5") + "," + gpsData.Longitude.ToString("F5") + "&zoom=14&size=640x640&type=hybrid&sensor=true&markers=color:blue%7Clabel:S%7C" + gpsData.Latitude + "," + gpsData.Longitude;
                    StartCoroutine(GetGoogleMap(new WWW(url), Renderer));
                }
                */
            }
            catch (Exception e)
            {
                TextLumens.text = "Error: " + e.Message;
            }
        }

        // Compass tracked based on camera movement
        direct.z = playerTransform.eulerAngles.y;

        // Calculating X,Y for Atan2
        X_heading = (Math.Cos(UserLat) * Math.Sin(Math.Abs(UserLong - TargetLong)));
        Y_heading = ((Math.Cos(UserLat) * Math.Sin(TargetLat))
            - (Math.Sin(UserLat) * Math.Cos(TargetLat) * Math.Cos(Math.Abs(UserLong - TargetLong))));

        // Calculates direction to target
        var targetAngle = Math.Atan2(X_heading, Y_heading);
        var CompassAngle = -targetAngle * Mathf.Rad2Deg + direct.z;

        // Calculating distance to target
        var lat = (TargetLat - UserLat) * (Math.PI / 180);
        var lng = (TargetLong - UserLong) * (Math.PI / 180);

        var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                      Math.Cos(UserLat * (Math.PI / 180)) * Math.Cos(TargetLat * (Math.PI / 180)) *
                      Math.Sin(lng / 2) * Math.Sin(lng / 2);

        //var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
        var h2 = 2 * Math.Atan2(Math.Sqrt(h1), Math.Sqrt(1 - h1));

        var distanceToTarget = Convert.ToInt32((5201 * h2)*1000);   // (5201*h2) = Kilometers in rio rancho
        //var distanceToTarget = Convert.ToInt32((6371 * h2)*1000); // Global average altitude

        if (distanceToTarget <= 5)
        {
            toggleArrow(2);
        }
        else if (distanceToTarget > 7)
        {
            toggleArrow(1);
        }

        // Compass points toward target
        transform.localEulerAngles = new Vector3(0, 0, Convert.ToSingle(CompassAngle));
        // Calculates and Displays distance to target
        //int targetDistant = Mathf.RoundToInt((Mathf.Sqrt(Mathf.Pow(xPosition, 2) + Mathf.Pow(yPosition, 2))));
        TargetHeading.text = message + distanceToTarget.ToString();

   
    }

    public void toggleArrow(int option)
    {

        switch(option)
        {
            case 1:
                HereSign.SetActive(false);
                
                TargetHeading.enabled = true;
                
                
                break;
            case 2:
                HereSign.SetActive(true);
                
                TargetHeading.enabled = false;
                
           
                break;
        }
    }

    public void togglePanel()
    {
        if (NaviMenu != null)
        {
            bool isActive = NaviMenu.activeSelf;

            NaviMenu.SetActive(!isActive);
        }
    }

    /*
    public double HaversineDistance()
    {

        var lat = (TargetLat - UserLat) * (Math.PI / 180);
        var lng = (TargetLong - UserLong) * (Math.PI/180);

        var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                      Math.Cos(UserLat *(Math.PI/180)) * Math.Cos(TargetLat * (Math.PI / 180)) *
                      Math.Sin(lng / 2) * Math.Sin(lng / 2);

        //var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
        var h2 = 2 * Math.Atan2(Math.Sqrt(h1),Math.Sqrt(1-h1));
        return 6371 * h2;
    }
    */

    public void changeTarget(int target)
    {   // Function to switch the position of the targets
        switch (target)
        {
            case 1: // Rover
                message = "Rover: ";
                TargetLat  =   35.29389200;
                TargetLong = -106.71549180;
                //TargetLat = 35.2938230;
                //TargetLong = -106.7155604;
                break;
            case 2: // Lander
                message = "Lander: ";
                TargetLat  =   35.29399750;
                TargetLong = -106.71561400;
                //TargetLat  =   35.29401300;
                //TargetLong = -106.71558050;
                break;
            case 3: // Sampling Site
                message = "Sampling: ";
                TargetLat  =   35.29353400;
                TargetLong = -106.71694150;
                //TargetLat  =   35.29356700;
                //TargetLong = -106.71688800;
                break;
        }
    }

    /*
    IEnumerator GetGoogleMap(WWW www, Image renderer)
    { // Used to render map image to user
        yield return www;
        //renderer.material.mainTexture = www.texture;

        if (www.texture != null)
        {
            Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(texture);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
            renderer.sprite = spriteToUse;
        }
        www.Dispose();
        www = null;
    }
    */

    // Voice commands function
    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    private void MoveQueuedEventsToExecuting()
    {
        lock (_queueLock)
        {
            while (_queuedData.Count > 0)
            {
                byte[] data = _queuedData[0];
                _processingData.Add(data);
                _queuedData.RemoveAt(0);
            }
        }
    }
}