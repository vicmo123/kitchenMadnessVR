using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toppingable : MonoBehaviour
{
    [HideInInspector] public bool isInIngredientReceiver;

    // Start is called before the first frame update
    void Start()
    {
        isInIngredientReceiver = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceivedInIngredientReceiver() {
        isInIngredientReceiver = true;
    }
}
