using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toppingable : MonoBehaviour
{
    [HideInInspector] public bool isInIngredientReceiver;
    public Taco.Ingredients ingredientType;
    Pickupable p;
    Rigidbody rb;

    public bool ready = false;

    // Start is called before the first frame update
    void Start()
    {
        p = this.GetComponent<Pickupable>();
        rb = this.GetComponent<Rigidbody>();
    }

    public void ReceivedInIngredientReceiver() {
        ready = false;
    }

    public void RemoveRigidbody() {
        Destroy(p);
        Destroy(rb);
    }
}
