using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ResetPositionToStart : MonoBehaviour
{
    [SerializeField]
    private ARSession arSession; // Assign this in the inspector if you need to reset the AR Session.
    
    [SerializeField]
    private Transform xrOrigin; // Assign the XR Origin transform here.

    public void ResetARPosition()
    {
        GameObject startObject = GameObject.Find("Anfang");
        if (startObject != null && xrOrigin != null)
        {
            xrOrigin.position = startObject.transform.position;
            xrOrigin.rotation = startObject.transform.rotation;

            // Optionally reset the AR Session to clear tracking data and restart tracking
            if (arSession != null)
            {
                arSession.Reset();
            }
        }
        else
        {
            Debug.LogWarning("ResetARPosition: 'Anfang' GameObject or XR Origin is not set.");
        }
    }
}
