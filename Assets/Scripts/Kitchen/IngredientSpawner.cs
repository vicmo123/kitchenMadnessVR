using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefab;

    [Tooltip("Time in seconds between each spawn of ingredient")] public float spawnCooldown = 3;

    public GameObject spawnPoint;
    public UnityEvent spawnEvent;
    private float timeForNextSpawn;

    private bool roundStarted;

    int ingredientsBeforeTortilla = 3;
    int initialIngredientsBeforeTortilla;

    public GameObject InvokeSpawn() {
        //Returns the game object to be able to acces it data
        return SpawnObject();
    }

    private GameObject SpawnObject() {
        int prefabIndex;
        if (ingredientsBeforeTortilla > 0) {
            ingredientsBeforeTortilla--;
            prefabIndex = Random.Range(0, prefab.Count);
        } else {
            prefabIndex = 0;
        }
        if (prefabIndex == 0) {
            ingredientsBeforeTortilla = initialIngredientsBeforeTortilla;
        }
        GameObject spawnedObject = GameObject.Instantiate<GameObject>(prefab[prefabIndex]);
        spawnedObject.transform.position = spawnPoint.transform.position;
        spawnEvent?.Invoke();
        return spawnedObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        roundStarted = false;
        initialIngredientsBeforeTortilla = ingredientsBeforeTortilla;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= timeForNextSpawn && roundStarted) {
            InvokeSpawn();
            timeForNextSpawn = Time.time + spawnCooldown;
        }
    }

    public void RoundStarting() {
        roundStarted = true;
        timeForNextSpawn = Time.time + spawnCooldown;
    }

    public void RoundEnding()
    {
        roundStarted = false;
    }
}
