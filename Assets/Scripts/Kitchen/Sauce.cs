using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sauce : MonoBehaviour
{

    public GameObject sauce;

    void Update() {
        pourSauce();
    }


    public void pourSauce() {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, 10)) {


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
        sauce.gameObject.SetActive(true);
       // float height = Vector3.Distance(hit.point, transform.position);
        sauce.transform.localScale = new Vector3(sauce.transform.localScale.x, hit.distance, sauce.transform.localScale.z);
        sauce.transform.localPosition = new Vector3(0, hit.distance/2, 0);
    }


}

