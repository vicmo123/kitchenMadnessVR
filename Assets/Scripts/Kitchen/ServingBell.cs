using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingBell : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
  
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("VRHANDS"))
        {
            SoundManager.BellDing?.Invoke();
           
        }
    }
}
