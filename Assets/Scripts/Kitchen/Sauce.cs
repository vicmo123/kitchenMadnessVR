using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sauce : MonoBehaviour
{
    
     public LineRenderer lr;
     
    private void Start()
    {
      
        //lr = gameObject.GetComponent<LineRenderer>();
     //  gameObject.SetActive(true);
    }
    void Update()
    {
        pourSauce();
    }


    public void pourSauce()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, 5))
        {


            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Counter"))
                {
                    // gameObject.SetActive(true);
                    lr.enabled = true;
                    saucePouring(hit);
                }


                else if (hit.collider.gameObject.CompareTag("Floor"))
                {
                    //gameObject.SetActive(true);
                    lr.enabled = true;
                    saucePouring(hit);
                }

                else if (hit.collider.gameObject.CompareTag("Taco")) {
                    //gameObject.SetActive(true);
                    hit.collider.gameObject.GetComponent<Taco>().ReceiveSauce();
                    lr.enabled = true;
                    saucePouring(hit);
                }

                else
                {

                    lr.enabled = false;
                    //GameObject.Destroy(lr);

                }

            }

        }
    }


        public void saucePouring(RaycastHit hit)
        {
            lr.startColor = Color.red;
            lr.endColor = Color.red;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, hit.point);
            lr.startWidth = 0.09f;
            lr.endWidth = 0.2f;
        }
    
    
}

