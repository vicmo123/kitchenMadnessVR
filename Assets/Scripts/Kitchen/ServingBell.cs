using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingBell : MonoBehaviour
{
    
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("VRHANDS"))
        {
            SoundManager.BellDing.Invoke();
           
        }
    }
}
