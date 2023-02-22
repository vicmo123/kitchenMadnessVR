using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingZone : MonoBehaviour
{
    public BoardManager boardManager;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TestTaco")
        {
            if (boardManager.isTacoGoodToServe(other.GetComponent<TestTaco>().GetRecipe()))
            {
                SoundManager.GoodJob?.Invoke();
            }
            else
            {
                boardManager.LoseOneStar();
                SoundManager.LooseStar?.Invoke();
            }

            GameObject.Destroy(other);
        }
        if (other.tag == "Taco")
        {
            if (!other.GetComponent<Pickupable>().isGrabbedByPlayer || !other.GetComponent<Pickupable>().isGrabbedByRat)
            {
                if (boardManager.isTacoGoodToServe(other.GetComponent<Taco>().SendTaco()))
                {
                    SoundManager.GoodJob?.Invoke();
                    boardManager.NbTacosServed++;
                }
                else
                {
                    boardManager.LoseOneStar();
                    SoundManager.LooseStar?.Invoke();

                }
                //taco goes away
                GameObject.Destroy(other);
            }
        }
    }
}
