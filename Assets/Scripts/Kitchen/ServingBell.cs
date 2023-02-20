using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingBell : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Right Hand") || other.gameObject.CompareTag("Left Hand"))
        {
            SoundManager.BellDing?.Invoke();
        }
    }
}
