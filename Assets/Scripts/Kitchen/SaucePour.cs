using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaucePour : MonoBehaviour
{
    public GameObject sauce;
    GameObject newSauce;

    // Update is called once per frame
    void FixedUpdate()
    {
        pourSauce();
    }

    public void pourSauce()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.up, out hit, 10))
        {


            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Counter"))
                {
                    newSauce = Instantiate(sauce, transform.position+transform.forward*hit.distance/2f, Quaternion.identity);
                    newSauce.transform.localScale = new Vector3(newSauce.transform.localScale.x, hit.distance, newSauce.transform.localScale.z);
                }

                else
                {
                    Destroy(newSauce);
                    newSauce = null;
                }

                
            }

        }
    }
}
