using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Defaults
    double UserLat;
    double UserLong;
    double TargetLat;
    double TargetLong;
    double X_heading;
    double Y_heading;

    public Text TargetHeading;

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
        changeTarget(2);
    }

    void Update()
    {
        MoveQueuedEventsToExecuting();
        while (_processingData.Count > 0)
        {
            var byteData = _processingData[0];
            _processingData.RemoveAt(0);
            try
            {
                var gpsData = GPS_DataPacket.ParseDataPacket(byteData);
                TextTime.text       = "     Time: " + DateTime.Now.ToString("t");
                TextLatitude.text   = " Latitude: " + gpsData.Latitude.ToString();
                TextLongitude.text  = "Longitude: " + gpsData.Longitude.ToString();
                TextLumens.text     = "   Lumens: " + gpsData.Lumens.ToString();
                GpsHeading.text    = "  Heading: " + gpsData.Heading.ToString();

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
                TextLatitude.text = "Error: " + e.Message;
            }
        }

        // Compass tracked based on camera movement
        direct.z = playerTransform.eulerAngles.y;

        X_heading = (Math.Cos(UserLat) * Math.Sin(Math.Abs(UserLong - TargetLong)));
        Y_heading = ((Math.Cos(UserLat) * Math.Sin(TargetLat))
            - (Math.Sin(UserLat) * Math.Cos(TargetLat) * Math.Cos(Math.Abs(UserLong - TargetLong))));

        // Calculates direction to target
        var targetAngle = Math.Atan2(X_heading, Y_heading);
        var CompassAngle = -targetAngle * Mathf.Rad2Deg + direct.z;

        // Compass points toward target
        transform.localEulerAngles = new Vector3(0, 0, Convert.ToSingle(CompassAngle));
        // Calculates and Displays distance to target
        //int targetDistant = Mathf.RoundToInt((Mathf.Sqrt(Mathf.Pow(xPosition, 2) + Mathf.Pow(yPosition, 2))));
        TargetHeading.text = "Target: " + targetAngle.ToString();

    }

    public void changeTarget(int target)
    {   // Function to switch the position of the targets
        switch (target)
        {
            // TODO: Change the targets to correct coordinates
            case 1: // Rover
                message = "Rover: ";
                TargetLat = 20;
                TargetLong = -10;
                break;
            case 2: // Lander
                message = "Lander: ";
                TargetLat  =   35.29401300;
                TargetLong = -106.71558050;
                break;
            case 3: // Sampling Site
                message = "Sampling Site: ";
                TargetLat  =   35.29356700;
                TargetLong = -106.71688800;
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