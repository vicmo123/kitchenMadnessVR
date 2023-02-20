using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    public float grillingMultiplier = 1;

    float initialMultiplier;
    int quantityGrilling;
    float minimumMultiplier;
    float nerfMultiplier;

    private void Start() {
        initialMultiplier = grillingMultiplier;
        quantityGrilling = 0;
        minimumMultiplier = .3f;
        nerfMultiplier = .2f;
    }

    private void Update() {
        grillingMultiplier = initialMultiplier - (quantityGrilling * nerfMultiplier);
        if (grillingMultiplier < minimumMultiplier) {
            grillingMultiplier = minimumMultiplier;
        }
    }

    private void OnTriggerStay(Collider other) {
        Burnable burnable = other.gameObject.GetComponentInParent<Burnable>();
        if (burnable != null) {
            burnable.OnGrill(this);
        }
    }

    private void OnTriggerEnter(Collider other) {
        quantityGrilling++;
    }

    private void OnTriggerExit(Collider other) {
        quantityGrilling--;
    }
}
