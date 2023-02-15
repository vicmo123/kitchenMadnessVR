using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sauce : MonoBehaviour
{
   
    void Update()
    {
        pourSauce();
    }


   public void pourSauce()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, 1))
        {

           
            if (hit.collider != null)
            {
                if(hit.collider.gameObject.CompareTag("Counter"))
                {
                    Debug.DrawRay(transform.position, -(transform.up), Color.red);
                }
                if (hit.collider.gameObject.CompareTag("Floor"))
                {
                    Debug.DrawRay(transform.position, -(transform.up), Color.red);
                }


            }

        }
        
    }
}
