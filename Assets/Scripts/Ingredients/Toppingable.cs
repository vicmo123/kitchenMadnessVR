using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toppingable : MonoBehaviour
{
    [HideInInspector] public bool isInIngredientReceiver;
    public Taco.Ingredients ingredientType;
    Pickupable p;
    Rigidbody rb;
    CuttableIngredient ci;
    [HideInInspector] public Mesh mesh;

    public bool ready = false;

    // Start is called before the first frame update
    void Start()
    {
        p = this.GetComponent<Pickupable>();
        rb = this.GetComponent<Rigidbody>();
        ci = this.GetComponent<CuttableIngredient>();
        //mesh = this.GetComponentInChildren<Mesh>();
    }

    public void ReceivedInIngredientReceiver() {
        ready = false;
    }

    public void RemoveRigidbody() {
        Destroy(p);
        Destroy(ci);
        Destroy(rb);
    }
}
