using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingArea : MonoBehaviour
{
    public BoardManager boardManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Taco")
        {
            if(!other.GetComponent<Pickupable>().isGrabbedByPlayer || !other.GetComponent<Pickupable>().isGrabbedByRat)
            {
                if (boardManager.isTacoGoodToServe(other.GetComponent<Taco>().SendTaco()) )
                {                
                    SoundManager.GoodJob?.Invoke();
                }
                else 
                {
                    SoundManager.LooseStar?.Invoke();

                }
                //taco goes away
                GameObject.Destroy(other);
            }
        }
    }
}
