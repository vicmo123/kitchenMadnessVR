using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Taco")
        {
            if(!other.GetComponent<Pickupable>().isGrabbedByPlayer || !other.GetComponent<Pickupable>().isGrabbedByRat)
            {
               // IngredientEnum recipe = other.GetComponent<Taco>().SendTaco();
            }
        }
    }
}
