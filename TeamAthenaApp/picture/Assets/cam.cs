using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.WSA.WebCam;
using UnityEngine.UI;
using System.IO;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
//voice code from Kam and Romogna
using UnityEngine.Windows.Speech;

public class cam : MonoBehaviour
{

    PhotoCapture photoCaptureObject = null;
    Texture2D targetTexture = null;
    Renderer quadRenderer;
    public Text Textd;

    //for display
    public RawImage ri;
    private Texture tt;
    private WebCamTexture camTexture;

    int capturedImageCount = 0;
    int maxphoto = 50;
    int h = 0;
    int w = 0;
    GameObject quad = null;
    // Use this for initialization
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    void Start()
    {
        tt = ri.texture;
        camTexture = new WebCamTexture(WebCamTexture.devices[0].name, 400, 400);
        camTexture.requestedHeight = 400;
        camTexture.requestedWidth = 400;
        camTexture.Play();
        ri.texture = camTexture;
        //camTexture.Stop();
        //ri = null;
        
        // Global Voice Command
        keywords.Add("picture", () =>
        {
            // Call the changeTarget function
            Debug.Log("cam");
           

            OpenCam();

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
    public void Start1()
    {
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
        targetTexture.hideFlags = HideFlags.HideAndDontSave;

        // Create a PhotoCapture object
        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject) {
            photoCaptureObject = captureObject;
            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 0.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            w = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            h = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            // Activate the camera
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result) {
                // Take a picture
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });
    }
    void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        Debug.Log("Saving......");
        Textd.text = "Saving... ";
        if (capturedImageCount < maxphoto)
        {
            Debug.Log("Saved Photo to the  disk!");
        }
        else
        {
            Debug.Log("Full!");
        }
    }
    void TakePicture()
    {
        capturedImageCount++;
        Debug.Log(string.Format("Taking Picture ({0}/{1})...", capturedImageCount, maxphoto));
        string filename = string.Format(@"QRImage{0}.jpg", capturedImageCount);
        string fPath = System.IO.Path.Combine(Application.persistentDataPath, filename);
        Debug.Log(fPath);
        photoCaptureObject.TakePhotoAsync(fPath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
    }

    public void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {

        TakePicture();
        // Copy the raw image data into our target texture
        /*photoCaptureFrame.UploadImageDataToTexture(targetTexture);

        // Create a gameobject that we can apply our texture to
        quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quadRenderer = quad.GetComponent<Renderer>() as Renderer;
        quadRenderer.material = new Material(Shader.Find("Unlit/Texture"));

        quad.transform.parent = this.transform;
        quad.transform.localPosition = new Vector3(100.0f, 0.0f, 3.0f);

        quadRenderer.material.SetTexture("_MainTex", targetTexture);*/
        photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemoryBYTE);
        // Deactivate our camera
        //photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        //photoCaptureFrame.Dispose();
    }

    public void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown our photo capture resource
        photoCaptureObject.Dispose();
        //System.Threading.Thread.Sleep(5000);
        //quadRenderer.material.SetTexture("_MainTex", null);
        targetTexture = null;
        photoCaptureObject = null;
        //quad.SetActive(false);
    }
    void OnCapturedPhotoToMemoryBYTE(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {
            List<byte> imageBufferList = new List<byte>();

            // Copy the raw IMFMediaBuffer data into our empty byte list.
            photoCaptureFrame.CopyRawImageDataIntoBuffer(imageBufferList);


            // In this example, we captured the image using the BGRA32 format.
            // So our stride will be 4 since we have a byte for each rgba channel.
            // The raw image data will also be flipped so we access our pixel data
            // in the reverse order.
            int stride = 4;
            float denominator = 1.0f / 255.0f;
            List<Color> colorArray = new List<Color>();
            for (int i = imageBufferList.Count - 1; i >= 0; i -= stride)
            {
                float a = (int)(imageBufferList[i - 0]) * denominator;
                float r = (int)(imageBufferList[i - 1]) * denominator;
                float g = (int)(imageBufferList[i - 2]) * denominator;
                float b = (int)(imageBufferList[i - 3]) * denominator;

                colorArray.Add(new Color(r, g, b, a));
            }

            colorArray.Reverse();

            // Converting the List<byte> to a bute array
            byte[] imageBufferArray = imageBufferList.ToArray();

            // Now we could do something with the array such as texture.SetPixels() or run image processing on the list

//#if !UNITY_EDITOR
        // create a barcode reader instance
        BarcodeReader barcodeReader = new BarcodeReader { AutoRotate = true };

        // Here use the Decoder class to decode the Qr code
        //Decoder.DecodeFromByteArray(imageBufferArray, cameraWidth, cameraHeight);

         try
     {
        // Locatable camera from the HoloLens use BGRA32 format in the MRCManager class 
        Result TextResult = barcodeReader.Decode(imageBufferArray,w,h, RGBLuminanceSource.BitmapFormat.BGRA32);
        //If QR code detected 
        if (TextResult != null)
        {
            Debug.Log("Result decoding: " + TextResult.Text);
                    Textd.text = "Result decoding: " + TextResult.Text;
            //
            // Do what you want with the result here
            //
        }
        //If QR code not detected
        else
        {
                    Textd.text = "No QR code detected";
                    Debug.Log("No QR code detected");
        }
                camTexture.Play();
            }
        //If error while decoding
     catch(System.Exception e)
     {
        Debug.Log("Exception:"+e);
     }





//#endif

        }
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }
    public void OpenCam()
    {
        camTexture.Stop();
        Start1();
        
        //photoCaptureObject = null;

    }

} // End class b1