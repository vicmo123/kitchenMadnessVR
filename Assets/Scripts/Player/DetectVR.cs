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
            //startingPosition = XROrigin.transform.rotation;
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
