using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    public void InvokeSpawn()
    {
        SpawnObject();
    }

    private void SpawnObject()
    {
        GameObject spawnedObject = GameObject.Instantiate<GameObject>(prefab);
        spawnedObject.transform.position = transform.position;
    }
}
