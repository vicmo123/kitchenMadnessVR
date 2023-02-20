using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    InterFace_Cutter currentlyCutting;
    // Update is called once per frame
    void FixedUpdate()
    {
        ObjectCutter();
    }

    public void ObjectCutter()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, -transform.forward, Color.red);
        if (Physics.Raycast(transform.position, -(transform.forward), out hit, .2f, LayerMask.GetMask("Food")))
        {
            InterFace_Cutter ic = hit.collider.GetComponent<InterFace_Cutter>();
            if (ic != null)
            {
                ic.Cut(hit);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(currentlyCutting)) 
        {
            currentlyCutting.StopCut();
            currentlyCutting = null;
        }
    }
}
