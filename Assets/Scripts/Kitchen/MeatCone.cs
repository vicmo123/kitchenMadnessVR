using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatCone : MonoBehaviour, InterFace_Cutter
{

    public GameObject meatIngredient;
    public Transform meatSpawn;

    public void Cut(RaycastHit hit)
    {
        Debug.Log("Cut started");
    }
    
    public void EndCut()
    {
        meatIngredient = Instantiate(meatIngredient, meatSpawn);
    }
}
