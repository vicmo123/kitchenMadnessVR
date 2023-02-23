using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorImpulse : MonoBehaviour
{
    private Rigidbody rb;
    public int force = 10;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        Throw();
    }

    private void Throw()
    {
        rb.AddRelativeForce(new Vector3(-force, 0, 0), ForceMode.Impulse);
    }
}
