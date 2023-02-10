using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    Transform cutterPosition;


    // Update is called once per frame
    void FixedUpdate()
    {
        ObjectCutter();
    }

    public void ObjectCutter()
    {
        RaycastHit hit;
        if (Physics.Raycast(cutterPosition.position, -(cutterPosition.forward), out hit, 0.4f, LayerMask.GetMask("Cuttable")))
        {
            Cuttable cuttable = hit.collider.gameObject.GetComponent<Cuttable>();
            if (cuttable != null)
            {
                cuttable.tryCut(hit);
            }
        }
    }
}
