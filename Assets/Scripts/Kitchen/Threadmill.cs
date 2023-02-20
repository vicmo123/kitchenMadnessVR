using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Threadmill : MonoBehaviour
{
    public float speed = 1;

    private void OnTriggerStay(Collider other) {
        float move = speed * Time.deltaTime;
        other.transform.position += new Vector3(move, 0, 0);
        //Debug.Log(move);
    }
}
