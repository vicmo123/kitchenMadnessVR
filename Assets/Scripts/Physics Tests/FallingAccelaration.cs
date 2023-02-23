using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingAccelaration : MonoBehaviour
{
    private Rigidbody rb;
    public int force = 10;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        FallFast();
    }

    private void FallFast()
    {
        rb.AddRelativeForce(new Vector3(0, -force, 0), ForceMode.Impulse);
    }
}
