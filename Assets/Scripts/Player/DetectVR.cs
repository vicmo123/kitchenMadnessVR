using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;
//using Unity.XR.CoreUtils;

public class DetectVR : MonoBehaviour
{
    public Transform XROrigin;
    float heightPC;
    static public Vector3 startingPosition;

    void Start()
    {
        float heightPC = 1.5f;

        var xrSettings = XRGeneralSettings.Instance;
        if (xrSettings == null)
        {
            return;
        }
        var xrManager = xrSettings.Manager;
        if (xrManager == null)
        {
            return;
        }
        var xrLoader = xrManager.activeLoader;
        if (xrLoader == null)
        {
            startingPosition = XROrigin.transform.position;
            startingPosition += new Vector3(0, heightPC, 0);
            return;
        }
        gameObject.SetActive(false);
    }
}
