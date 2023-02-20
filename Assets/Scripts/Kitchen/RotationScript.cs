using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    float meatRotation;
    void Update()
    {
        meatRotation += 0.01f * Time.deltaTime;
        gameObject.transform.Rotate(0, meatRotation, 0);
    }
}
