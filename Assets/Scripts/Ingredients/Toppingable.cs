using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toppingable : MonoBehaviour
{
    [HideInInspector] public bool isInIngredientReceiver;
    public Taco.Ingredients ingredientType;
    Rigidbody rb;

    [HideInInspector] public bool ready;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceivedInIngredientReceiver() {
        ready = false;
    }

    public void RemoveRigidbody() {
        Destroy(rb);
    }
}
