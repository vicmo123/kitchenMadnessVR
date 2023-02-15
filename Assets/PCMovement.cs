using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMovement : MonoBehaviour
{
    public Transform player;

    public float speed = 2f;

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (Input.GetKey(KeyCode.W))
        {
            player.transform.Translate(Vector3.forward * Time.deltaTime * speed);
            Debug.Log("Foward");
        }
        player.transform.position = new Vector3(player.position.x, DetectVR.startingPosition.y, player.position.z);
    }
}
