﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.WSA.WebCam;
using UnityEngine.UI;
using System.IO;



public class b1 : MonoBehaviour
{

    PhotoCapture photoCaptureObject = null;
    Texture2D targetTexture = null;
    Renderer quadRenderer;
    int capturedImageCount = 0;
    int maxphoto = 50;
    GameObject quad = null;
    // Use this for initialization
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
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
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
        string filename = string.Format(@"CapturedImage{0}.jpg", capturedImageCount);
        string fPath = System.IO.Path.Combine(Application.persistentDataPath, filename);
        Debug.Log(fPath);
        photoCaptureObject.TakePhotoAsync(fPath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
    }

    public void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {

        TakePicture();
        // Copy the raw image data into our target texture
        photoCaptureFrame.UploadImageDataToTexture(targetTexture);

        // Create a gameobject that we can apply our texture to
        quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quadRenderer = quad.GetComponent<Renderer>() as Renderer;
        quadRenderer.material = new Material(Shader.Find("Unlit/Texture"));

        quad.transform.parent = this.transform;
        quad.transform.localPosition = new Vector3(38.0f, 0.0f, 3.0f);

        quadRenderer.material.SetTexture("_MainTex", targetTexture);

        // Deactivate our camera
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        //photoCaptureFrame.Dispose();
    }

    public void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown our photo capture resource
        photoCaptureObject.Dispose();
        //System.Threading.Thread.Sleep(5000);
        quadRenderer.material.SetTexture("_MainTex", null);
        targetTexture = null;
        photoCaptureObject = null;
        quad.SetActive(false);
    }

    public void OpenCam()
    {

        Start1();
        photoCaptureObject = null;

    }

} // End class b1



