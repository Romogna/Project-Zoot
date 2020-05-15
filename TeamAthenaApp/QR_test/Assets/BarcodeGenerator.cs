using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
using ZXing.Common;
using System.Threading;
[RequireComponent(typeof(RawImage))]

public class BarcodeGenerator : MonoBehaviour
{
    [SerializeField] private BarcodeFormat format = BarcodeFormat.QR_CODE;
    [SerializeField] private string data = "test";
    [SerializeField] private int width = 512;
    [SerializeField] private int height = 512;

    public RawImage cRawImage;

    private void Start()
    {
        //cRawImage = GetComponent();
        cRawImage = GetComponent<RawImage>();

        // Generate the texture
        Texture2D tex = GenerateBarcode(data, format, width, height);

        // Setup the RawImage
        cRawImage.texture = tex;
        cRawImage.rectTransform.sizeDelta = new Vector2(tex.width, tex.height);
    }

    private Texture2D GenerateBarcode(string data, BarcodeFormat format, int width, int height)
    {
        // Generate the BitMatrix
        BitMatrix bitMatrix = new MultiFormatWriter().encode(data, format, width, height);

        // Generate the pixel array
        Color[] pixels = new Color[bitMatrix.Width * bitMatrix.Height];
        int pos = 0;
        for (int y = 0; y < bitMatrix.Height; y++)
        {
            for (int x = 0; x < bitMatrix.Width; x++)
            {
                pixels[pos++] = bitMatrix[x, y] ? Color.black : Color.white;
            }
        }

        // Setup the texture
        Texture2D tex = new Texture2D(bitMatrix.Width, bitMatrix.Height);
        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }

 /**
    public Result decode(Uri uri)
    {
        Bitmap image;
        try
        {
            image = (Bitmap)Bitmap.FromFile(uri.LocalPath);
        }
        catch (Exception)
        {
            throw new FileNotFoundException("Resource not found: " + uri);
        }

        using (image)
        {
            LuminanceSource source;
            source = new BitmapLuminanceSource(image);
            BinaryBitmap bitmap = new BinaryBitmap(new HybridBinarizer(source));
            Result result = new MultiFormatReader().decode(bitmap);
            if (result != null)
            {
                //... code found
            }
            else
            {
                //... no code found
            }
            return result;
        }
    }
**/
}