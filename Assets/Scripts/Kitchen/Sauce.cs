using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sauce : MonoBehaviour
{

    public GameObject sauce;

    private void Start() {

        //lr = gameObject.GetComponent<LineRenderer>();
        //  gameObject.SetActive(true);
    }
    void Update() {
        pourSauce();
    }


    public void pourSauce() {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, 5)) {


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

                    sauce.gameObject.active = false;
                    //GameObject.Destroy(lr);

                }

            }

        }
    }


    public void saucePouring(RaycastHit hit) {
        sauce.gameObject.active = true;
        float height = Vector3.Distance(hit.point, transform.position);
        sauce.transform.localScale = new Vector3(sauce.transform.localScale.x, height, sauce.transform.localScale.z);
        sauce.transform.localPosition = new Vector3(0, height / 2, 0);
    }


}

