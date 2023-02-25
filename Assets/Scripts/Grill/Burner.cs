using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    public List<GameObject> grillingObjects;

    public float grillingMultiplier = 1;

    float initialMultiplier;
    float minimumMultiplier;
    float nerfMultiplier;

    private void Start() {
        grillingObjects = new List<GameObject>();

        initialMultiplier = grillingMultiplier;
        minimumMultiplier = .3f;
        nerfMultiplier = .2f;

        SoundManager.Grilling?.Invoke();
    }

    private void Update() {
        grillingMultiplier = initialMultiplier - (grillingObjects.Count * nerfMultiplier);
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
        GameObject go = other.transform.root.gameObject;
        if (!grillingObjects.Contains(go)) {
            grillingObjects.Add(go);
        }
    }

    private void OnTriggerExit(Collider other) {
        GameObject go = other.transform.root.gameObject;
        if (grillingObjects.Contains(go)) {
            grillingObjects.Remove(go);
        }
    }
}
