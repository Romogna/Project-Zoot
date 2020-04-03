using System.Collections;
using UnityEngine;
using UnityEngine.UI;
// Voice
using UnityEngine.Windows.Speech;
using System.Linq;
using System.Collections.Generic;
// Voice

public class Compass : MonoBehaviour
{
    // Camera transform
    public Transform playerTransform;
    Vector3 direct;

    // Defaults
    public int xPosition;
    public int yPosition;
    public Text txt;
    string message = "Default: ";

    // Speech
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for using keywords to activate functions
    void Start()
    {
        // Global Voice Command
        keywords.Add("Target Rover", () => 
        {
            // Call the changeTarget function
            changeTarget(1);
        });
        keywords.Add("Target Lander", () =>
        {
            changeTarget(2);
        });
        keywords.Add("Target Sampling", () =>
        {
            changeTarget(3);
        });
        keywords.Add("Compass Toggle", () =>
        {
            toggleArrow();
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
    }

    public void changeTarget(int target)
    {   // Function to switch the position of the targets
        switch (target)
        {
            // These would change to sensor data
            case 1: // Rover
                message = "Rover: ";
                xPosition = 20;
                yPosition = -10;
                break;
            case 2: // Lander
                message = "Lander: ";
                xPosition = -5;
                yPosition = -30;
                break;
            case 3: // Sampling Site
                message = "Sampling Site: ";
                xPosition = -10;
                yPosition = 20;
                break;
        }
    }

    // This is how to access the imu of the HMD
    private void Update()
    {        
        // Compass tracked based on camera movement
        direct.z = playerTransform.eulerAngles.y;

        // Calculates direction to target
        var targetAngle = -Mathf.Atan2(xPosition, yPosition) * Mathf.Rad2Deg + direct.z;

        // Compass points toward target
        transform.localEulerAngles = new Vector3(0, 0, targetAngle);

        // Calculates and Displays distance to target
        int targetDistant = Mathf.RoundToInt((Mathf.Sqrt(Mathf.Pow(xPosition, 2) + Mathf.Pow(yPosition, 2))));
        txt.text = message + targetDistant.ToString();
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


    // Toggle function
    public GameObject NaviMenu;

    public void togglePanel()
    {
        if (NaviMenu != null)
        {
            bool isActive = NaviMenu.activeSelf;

            NaviMenu.SetActive(!isActive);
        }
    } // 

    // Toggle Compass
    public GameObject SpriteArrow;
    public Text distant;

    public void toggleArrow()
    {
        if (SpriteArrow == null)
        {
            return;
        }
        bool isActive = SpriteArrow.activeSelf;

        distant.enabled = !isActive;
        SpriteArrow.SetActive(!isActive);
    }

}