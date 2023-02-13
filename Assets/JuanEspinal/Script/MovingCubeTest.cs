using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCubeTest : MonoBehaviour
{
    enum Directions
    {
        GOING_LEFT, GOING_RIGH
    }
    Directions moving = Directions.GOING_LEFT;
    // Start is called before the first frame update
    void Start()
    {
        Directions moving = Directions.GOING_LEFT;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Pickupable>().isGrabbedByPlayer)
        {
            switch (moving)
            {
                case Directions.GOING_LEFT:
                    transform.Translate(Vector3.right * Time.deltaTime);
                    if (transform.position.x >= .4)
                        moving = Directions.GOING_RIGH;
                    break;
                case Directions.GOING_RIGH:
                    transform.Translate(Vector3.left * Time.deltaTime);
                    if (transform.position.x <= -.4)
                        moving = Directions.GOING_LEFT;
                    break;
            }
        }
    }
}
