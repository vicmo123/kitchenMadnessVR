using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;
//using Unity.XR.CoreUtils;

public class DetectVR : MonoBehaviour
{
    public Transform XROrigin;
    //public Transform rightHand;
    //bool hasStarted = false;
    static public Vector3 startingPosition = new Vector3(0, 1.8f, 0);

    void Start()
    {
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
            XROrigin.transform.position = startingPosition;
            return;
        }
        gameObject.SetActive(false);
    }
    private void Update()
    {
        //if (!hasStarted)
        //{
        //    Debug.Log("Started");
        //    hasStarted = true;
        //    rightHand.position = new Vector3(startingPosition.x + .2f, startingPosition.y - .2f, startingPosition.z + .2f);
        //}
    }
}
