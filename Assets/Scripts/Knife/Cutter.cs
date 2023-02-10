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
        if (Physics.Raycast(cutterPosition.position, -(cutterPosition.forward), 0.4f, LayerMask.GetMask("Cuttables")))
        {
            Debug.Log("Ingredient cut");
            //Fucntion call for spliting the ingredient object;
        }
    }
}
