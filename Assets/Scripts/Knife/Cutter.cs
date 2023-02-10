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
        if (Physics.Raycast(transform.position, -(transform.forward), out hit, 1, LayerMask.GetMask("Cuttable")))
        {
            Cuttable cuttable = hit.collider.gameObject.GetComponent<Cuttable>();
            if (cuttable != null)
            {
                cuttable.tryCut(hit);
            }
        }
    }
}
