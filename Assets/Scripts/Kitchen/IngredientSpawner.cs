using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefab;

    [Tooltip("Time in seconds between each spawn of ingredient")] public float spawnCooldown = 3;

    public GameObject spawnPoint;

    private float timeForNextSpawn;

    public GameObject InvokeSpawn() {
        //Returns the game object to be able to acces it data
        return SpawnObject();
    }

    private GameObject SpawnObject() {
        int prefabIndex = Random.Range(0, prefab.Count);
        GameObject spawnedObject = GameObject.Instantiate<GameObject>(prefab[prefabIndex]);
        spawnedObject.transform.position = spawnPoint.transform.position;

        return spawnedObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        timeForNextSpawn = Time.time + spawnCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= timeForNextSpawn) {
            InvokeSpawn();
            timeForNextSpawn = Time.time + spawnCooldown;
        }
    }
}
