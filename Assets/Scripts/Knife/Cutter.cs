using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour
{

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
            if(ic!=null)
            {
                ic.Cut(hit);
               
            }

           
        }



    }
}
