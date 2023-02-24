using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    InterFace_Cutter currentlyCutting;
    public GameObject raycastSource;
    // Update is called once per frame
    void FixedUpdate()
    {
        ObjectCutter();
    }

    public void ObjectCutter()
    {
        RaycastHit hit;
        Debug.DrawRay(raycastSource.transform.position, -raycastSource.transform.forward);
        if (Physics.Raycast(transform.position, -(transform.forward), out hit, .2f, LayerMask.GetMask("Food")))
        {
            InterFace_Cutter Icut = hit.collider.GetComponent<InterFace_Cutter>();
            if (currentlyCutting != null && !currentlyCutting.Equals(Icut))
                currentlyCutting.StopCut();
            currentlyCutting = Icut;

            if (Icut != null)
            {
                Icut.Cut(hit);
            }
        }
        else if (currentlyCutting != null)
        {
            currentlyCutting.StopCut();
            currentlyCutting = null;
        }
    }
}
