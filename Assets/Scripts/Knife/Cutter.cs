using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    InterFace_Cutter currentrlyCutting;

    // Update is called once per frame
    void FixedUpdate()
    {
        ObjectCutter();
    }

    public void ObjectCutter()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -(transform.forward), out hit, .2f, LayerMask.GetMask("Food")))
        {

            currentrlyCutting = hit.collider.GetComponent<InterFace_Cutter>();
            if (currentrlyCutting != null)
            {
                currentrlyCutting.Cut(hit);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        InterFace_Cutter otherCutter = other.gameObject.GetComponent<InterFace_Cutter>();
        if (currentrlyCutting.Equals(otherCutter))
        {
            otherCutter.StopCut();
        }
    }
}
