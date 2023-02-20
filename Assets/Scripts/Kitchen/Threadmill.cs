using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Threadmill : MonoBehaviour
{
    public float speed = 1;
    private GameObject lastGO;
    private Collider lastCollider;
    private void OnTriggerStay(Collider other) {
        float move = speed * Time.deltaTime;
        if (CheckSameObjectDiferentCollider(other))
            return;
        lastCollider = other;
        lastGO = other.gameObject;
        other.GetComponentInParent<Transform>().position += new Vector3(move, 0, 0);
        //Debug.Log(move);
    }

    private bool CheckSameObjectDiferentCollider(Collider other)
    {
        if (!other.gameObject.Equals(lastGO))
            return false;
        
        if (other.Equals(lastCollider))
            return false;
        else
            return true;
    }
}
