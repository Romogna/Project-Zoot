/**
 * QR scanner
 * Kam/Gw/Richard
 * Cited from Adrian M. Nenu
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
//using System.Threading.Tasks;
using System;

public class ZxingTest : MonoBehaviour
{
    private Rect screenRect;
    private WebCamTexture camTexture;
    public RawImage ri;
    private Texture tt;



    void Start()
    {
        /*screenRect = new Rect(0, 0, 400, 400);
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = 400;
        camTexture.requestedWidth = 400;
        if (camTexture != null)
        {
            camTexture.Play();
           
            
        }*/
        tt = ri.texture;
        camTexture = new WebCamTexture(WebCamTexture.devices[0].name,400,400);
        camTexture.requestedHeight = 400;
        camTexture.requestedWidth = 400;
        camTexture.Play();
        ri.texture = camTexture;
    }

    /*void OnGUI()
    {
        // drawing the camera on screen
        GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleToFit);
        // do the reading — you might want to attempt to read less often than you draw on the screen for performance sake

        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            // decode the current frame
            var result = barcodeReader.Decode(camTexture.GetPixels32(),
              camTexture.width, camTexture.height);
            if (result != null)
            {
                Debug.Log("DECODED TEXT FROM QR: " + result.Text);
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }

    }
*/
}

