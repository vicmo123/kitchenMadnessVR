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

    private void OnCollisionStay(Collision collision) {
        Burnable burnable = collision.gameObject.GetComponent<Burnable>();
        if (burnable != null) {
            burnable.OnGrill(this);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        quantityGrilling++;
    }

    private void OnCollisionExit(Collision collision) {
        quantityGrilling--;
    }
}
