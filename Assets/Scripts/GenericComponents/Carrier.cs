using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour
{
    //public Transform attachPoint; ATTACH POINT NOT WORKING PROPERLY
    [HideInInspector] public bool holdingItem;

    private void Start()
    {
        holdingItem = false;
    }

    public void DropItem()
    {
        if (holdingItem)
        {
            holdingItem = false;
            Pickupable[] pickupables = gameObject.GetComponentsInChildren<Pickupable>();
            GameObject model = gameObject.transform.GetChild(0).gameObject;

            transform.DetachChildren();

            int i = 0;
            foreach(Pickupable P in pickupables) //To Skip Parent, NEED TO FIND A BETTER WAY
            {
                if (i == 0)
                {
                    model.transform.SetParent(transform);
                }
                else
                {
                    holdingItem = false;
                    P.DropItem();
                }
                i++;
            }
        }
    }
}
