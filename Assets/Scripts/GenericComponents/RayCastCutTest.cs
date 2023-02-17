using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastCutTest : MonoBehaviour
{
    InterFace_Cutter currentlyCutting;
    private void FixedUpdate()
    {
        RayCastCut();
    }

    void RayCastCut()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up,out hit, 1, LayerMask.GetMask("Cuttable")))
        {
            currentlyCutting = hit.collider.gameObject.GetComponent<InterFace_Cutter>();
            if (currentlyCutting != null)
                currentlyCutting.Cut(hit);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InterFace_Cutter cutObject = other.gameObject.GetComponent<InterFace_Cutter>();
        if (cutObject.Equals(currentlyCutting))
            cutObject.StopCut();
    }
}
