using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;
//using Unity.XR.CoreUtils;

public class DetectVR : MonoBehaviour
{
    public Transform cameraOffset;

    void Start()
    {

        var xrSettings = XRGeneralSettings.Instance;
        if (xrSettings == null)
        {
            Debug.Log("xrSettings is null");
            return;
        }
        var xrManager = xrSettings.Manager;
        if (xrManager == null)
        {
            Debug.Log("xrManager is null");
            return;
        }
        var xrLoader = xrManager.activeLoader;
        if (xrLoader == null)
        {
            Debug.Log("xrLoader is null");
            cameraOffset.transform.position = new Vector3(0, .7f, 0);
            return;
        }

        Debug.Log("VR set Connected");
        gameObject.SetActive(false);
    }
}
