using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMovement : MonoBehaviour
{
    public Transform player;

    public float fowardBackwardSpeed = 2f;
    public float leftRightSpeed = 1f;

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (Input.GetKey(KeyCode.W))
        {
            player.transform.Translate(Vector3.forward * Time.deltaTime * fowardBackwardSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.transform.Translate(Vector3.back * Time.deltaTime * fowardBackwardSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.transform.Translate(Vector3.left * Time.deltaTime * leftRightSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.transform.Translate(Vector3.right * Time.deltaTime * leftRightSpeed);
        }
        player.transform.position = new Vector3(player.position.x, DetectVR.startingPosition.y, player.position.z);
    }
}
