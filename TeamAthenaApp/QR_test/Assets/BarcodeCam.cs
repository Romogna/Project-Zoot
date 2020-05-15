// Threading
using System.Threading.Tasks;
// Aux
using System.Collections.Generic;
//Speech
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
// QR Reader
using ZXing;
using ZXing.QrCode;


public class BarcodeCam : MonoBehaviour
{
    // Texture for encoding test
    public Texture2D encoded;

    private WebCamTexture camTexture;

    private Color32[] cam;
    private int W, H;

    private Rect screenRect;

    private bool isQuit;

    public string LastResult;

    private bool shouldEncodeNow;

    // Speech
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    //Task qrThread = new Task(Print());

    void OnGUI()
    {
        GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleToFit);
    }

    void OnEnable()
    {
        if (camTexture != null)
        {
            camTexture.Play();
            W = camTexture.width;
            H = camTexture.height;
        }
    }
    void OnDisable()
    {
        if (camTexture != null)
        {
            camTexture.Pause();
        }
    }

    void OnDestroy()
    {
        //qrThread.Sleep();
        camTexture.Stop();
    }
    /*
    // It's better to stop the thread by itself rather than abort it.
    void OnApplicationQuit()
    {
        isQuit = true;
    }
    */
    void Start()
    {
        //Keywords();
        encoded = new Texture2D(256, 256);
        LastResult = "{Placeholder}";
        shouldEncodeNow = true;

        screenRect = new Rect(0, 0, Screen.width, Screen.height);

        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height; // 480;
        camTexture.requestedWidth = Screen.width; //640;
        OnEnable(); // Use voice control

        //Task.Run(() => Print());
        Task.Run(() => DecodeQR());         
    }

    void Update()
    {
        if (cam == null)
        {
            cam = camTexture.GetPixels32();
        }

        // encode the last found
        var textForEncoding = LastResult;
        if (shouldEncodeNow &&
            textForEncoding != null)
        {
            var color32 = Encode(textForEncoding, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
            shouldEncodeNow = false;
        }
    }
    /*
    async Task Print()
    {
        Debug.Log("Working?");
        Task.Delay(1000).Wait();
        await Print();
    }
    */
    private Task DecodeQR()
    {
        // create a reader with a custom luminance source
        var barcodeReader = new BarcodeReader { AutoRotate = false, TryHarder = false };
        Debug.Log("Decoding");

        while (true)
        {

            try
            {
                // decode the current frame
                var result = barcodeReader.Decode(cam, W, H);
                Debug.Log("Trying");
                if (result != null)
                {
                    Debug.Log("Result not Null");
                    LastResult = result.Text;
                    shouldEncodeNow = true;
                    return Task.Run(() => { Debug.Log(result.Text); });
                }

                // Sleep a little bit and set the signal to get the next frame
                //Thread.Sleep(200);
                Task.Delay(1000).Wait();
                cam = null;
            }
            catch
            {
                Debug.Log("Catch");
                Task.Delay(1000).Wait();
                return Task.Run(() => { Debug.Log("failed"); });
            }
        }
    }
    
    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    // Keyword library
    void Keywords()
    {
        // Global Voice Command
        keywords.Add("Camera Enable", () =>
        {
            // Call the changeTarget function
            OnEnable();
        });
        keywords.Add("Camera Disable", () =>
        {
            // Call the changeTarget function
            OnDisable();
        });
        keywords.Add("Minimize Info", () =>
        {
            //toggleArrow();
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
}