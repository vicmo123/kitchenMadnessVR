using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauceBottleSound : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.ToolDropped?.Invoke();
    }
}
