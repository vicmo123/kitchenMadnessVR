using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    public GameObject InvokeSpawn()
    {
        //Returns the game object to be able to acces it data
        return SpawnObject();
    }

    private GameObject SpawnObject()
    {
        GameObject spawnedObject = GameObject.Instantiate<GameObject>(prefab, transform.position, Quaternion.identity);
        spawnedObject.GetComponent<Rat>().agent.Warp(transform.position);

        return spawnedObject;
    }
}
