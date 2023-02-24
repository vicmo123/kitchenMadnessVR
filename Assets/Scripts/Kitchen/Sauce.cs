using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sauce : MonoBehaviour
{

    public GameObject sauce;



    void FixedUpdate()
    {
        pourSauce();
    }


    public void pourSauce() {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, 100)) {


            if (hit.collider != null) {
                if (hit.collider.gameObject.CompareTag("Counter")) {
                    saucePouring(hit);
                }


                else if (hit.collider.gameObject.CompareTag("Floor")) {
                    saucePouring(hit);
                }

                else if (hit.collider.gameObject.CompareTag("Taco")) {
                    saucePouring(hit);
                    hit.collider.gameObject.GetComponent<Taco>().ReceiveSauce();
                }

                else {

                    sauce.gameObject.SetActive(false);


                }

            }

        }
    }
    

    public void saucePouring(RaycastHit hit) {
        SoundManager.SauceSquirt?.Invoke();
        sauce.gameObject.SetActive(true);
        Vector3 midPoint = (transform.position + hit.point) / 2f;
        float yScale = ((hit.distance) / transform.parent.localScale.y) / 2;
        sauce.transform.localScale = new Vector3(sauce.transform.localScale.x, yScale, sauce.transform.localScale.z);
        sauce.transform.position = midPoint;
    }


}

