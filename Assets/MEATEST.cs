using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MEATEST : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeatCone meatCOne = GameObject.FindObjectOfType<MeatCone>();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 9999))
        {
            meatCOne.Cut(hit);
            meatCOne.EndCut();
        }


        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
