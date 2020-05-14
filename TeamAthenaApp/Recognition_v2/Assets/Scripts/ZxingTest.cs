using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using System.Threading.Tasks;

public class ZxingTest : MonoBehaviour
{
    public Text informationText;
    private bool doDecoding;
    private WebCamTexture camTexture;
    private Rect screenRect;
    private Color32[] cam;
    private int W, H;
    public string info_string;

    void OnEnable()
    {
        if (camTexture != null)
        {
            camTexture.Play();
            W = camTexture.width;
            H = camTexture.height;
            Debug.Log("Camera On");
            doDecoding = true;
        }
        Debug.Log("CamTexture = " + (camTexture != null));
    }
    void OnDisable()
    {
        if (camTexture != null)
        {
            camTexture.Pause();
            doDecoding = false;
        }
    }
    void OnGUI()
    {
        GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleToFit); //ScaleMode.ScaleToFit
    }
    // Start is called before the first frame update
    void Start()
    { 
        screenRect = new Rect(0, 0, 640, 480); //Screen.width, Screen.height
        camTexture = new WebCamTexture
        {
            requestedHeight = Screen.height, // 480; Screen.height
            requestedWidth = Screen.width //640; Screen.width
        };
        OnEnable();
        Task.Run(() => Decode());
    }
    // Update is called once per frame
    void Update()
    {
        if (cam == null)
        {
            cam = camTexture.GetPixels32();
        }
        else
        {
            Debug.Log("Camera is null");
        }
    }

    [System.Obsolete]
    private async Task Decode()
    {
        // Create Barcode decoder
        var QRReader = new BarcodeReader
        {
            PossibleFormats = new List<BarcodeFormat>()
        };
        QRReader.PossibleFormats.Add(BarcodeFormat.QR_CODE);
        QRReader.AutoRotate = false;
        QRReader.TryHarder = false;

        while (doDecoding)
        {
            Debug.Log("Trying to Decode");
            //await Task.Delay(2000);
            try
            {
                // Decode result
                var info = QRReader.Decode(cam, H, W);

                Debug.Log("Found: " + info.ToString());
                /*
                if (info != null)
                {
                    info_string = info.ToString();
                    Debug.Log("Info = " + info_string);

                    return "Success";
                }
                else
                {
                    Debug.Log("Info/null = " + "Nothing");
                    return "Info = null";
                }
                */
            }
            catch
            {
                Debug.Log("Failed to Decode");
                return "Catch: Failed to decode";
            }
            
        }
        return "Done";
    }
}
/*
Task DecodeQR()
{
    // create a reader with a custom luminance source
    var barcodeReader = new BarcodeReader { AutoRotate = false, TryHarder = false };
    Debug.Log("Decoding");

    while (true)
    {
        Debug.Log("True");
        if (isQuit)
            break;

        try
        {
            // decode the current frame
            var result = barcodeReader.Decode(c, W, H);
            Debug.Log("Trying");
            if (result != null)
            {
                Debug.Log("Result not Null");
                LastResult = result.Text;
                shouldEncodeNow = true;
                Debug.Log(result.Text);
            }

            // Sleep a little bit and set the signal to get the next frame
            //Thread.Sleep(200);
            System.Threading.Tasks.Task.Delay(1000).Wait();
            c = null;
        }
        catch
        {
            Debug.Log("Catch");
        }
    }
}
*/