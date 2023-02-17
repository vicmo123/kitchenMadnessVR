using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastCutTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        RayCastCut();
    }

    void RayCastCut()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up,out hit, 1, LayerMask.GetMask("Cuttable")))
        {
            Cuttable cutObject = hit.collider.gameObject.GetComponent<Cuttable>();
            cutObject.Cut(hit);
        }
    }
}
