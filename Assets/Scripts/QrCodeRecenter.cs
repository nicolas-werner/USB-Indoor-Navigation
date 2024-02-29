using Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;
using UnityEngine.UI;
using TMPro;


public class QrCodeRecenter : MonoBehaviour
{
    [SerializeField]
    private ARSession session;
    [SerializeField]
    private XROrigin sessionOrigin;
    [SerializeField]
    private ARCameraManager cameraManager;
    [SerializeField]
    private List<Target> navigationTargetObjects = new List<Target>();

    private Texture2D cameraImageTexture;
    private IBarcodeReader reader = new BarcodeReader(); // Create a barcode reader instance
    private bool isScanningEnabled = false; // Controls whether scanning is active

public TMP_Text scanningMessageText;
    private void OnEnable()
    {
        cameraManager.frameReceived += OnCameraFrameReceived;
    }

    private void OnDisable()
    {
        cameraManager.frameReceived -= OnCameraFrameReceived;
    }

    // Method to toggle QR code scanning
    public void ToggleScanning()
    {
        isScanningEnabled = !isScanningEnabled;
        if (isScanningEnabled)
        {
            Debug.Log("QR Scanning Enabled");
            StartCoroutine(ShowMessage("Scanning Activated", 2));

        }
        else
        {
            Debug.Log("QR Scanning Disabled");
            StartCoroutine(ShowMessage("Scanning Deactivated", 2));
        }
    }
    IEnumerator ShowMessage(string message, float delay)
    {
        scanningMessageText.text = message; // Set the message text
        scanningMessageText.gameObject.SetActive(true); // Make the text element visible

        yield return new WaitForSeconds(delay); // Wait for the specified delay

        scanningMessageText.gameObject.SetActive(false); // Hide the text element
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        if (!isScanningEnabled) return; // Early exit if scanning is not enabled

        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            return;
        }

        var conversionParams = new XRCpuImage.ConversionParams
        {
            // Get the entire image
            inputRect = new RectInt(0, 0, image.width, image.height),
            // Downsample by 2
            outputDimensions = new Vector2Int(image.width / 2, image.height / 2),
            // Choose RGBA format
            outputFormat = TextureFormat.RGBA32,
            // Flip across the vertical axis (mirror image)
            transformation = XRCpuImage.Transformation.MirrorY
        };

        int size = image.GetConvertedDataSize(conversionParams);
        var buffer = new NativeArray<byte>(size, Allocator.Temp);
        image.Convert(conversionParams, buffer);
        image.Dispose();

        cameraImageTexture = new Texture2D(
            conversionParams.outputDimensions.x,
            conversionParams.outputDimensions.y,
            conversionParams.outputFormat,
            false);

        cameraImageTexture.LoadRawTextureData(buffer);
        cameraImageTexture.Apply();
        buffer.Dispose();

        var result = reader.Decode(cameraImageTexture.GetPixels32(), cameraImageTexture.width, cameraImageTexture.height);
        if (result != null)
        {
            SetQrCodeRecenterTarget(result.Text);
        }
    }

    private void SetQrCodeRecenterTarget(string targetText)
    {
        Target currentTarget = navigationTargetObjects.Find(x => x.Name.ToLower().Equals(targetText.ToLower()));
        if (currentTarget != null)
        {
            // Reset position and rotation of ARSession
            session.Reset();

            // Add offset for recentering
            sessionOrigin.transform.position = currentTarget.PositionObject.transform.position;
            sessionOrigin.transform.rotation = currentTarget.PositionObject.transform.rotation;
        }
    }
}
