using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DinosaurManager : MonoBehaviour
{
    public GameObject DinoPrefab;
    public Transform ServiceZone;
    private CountDownTimer timer;
    private float duration = 10.0f;
    private Vector2 timeRange = new Vector2(2.0f, 15.0f);
    public float spawnRadius;
    public bool roundActive { get; set; } = false;

    private void Awake()
    {
        timer = new CountDownTimer(duration, true);
        timer.StartTimer();
    }

    private void Start()
    {
        timer.OnTimeIsUpLogic = () =>
        {
            duration = Random.Range(timeRange.x, timeRange.y);
            timer.SetDuration(duration);
        };
    }

    private void Update()
    {
        if (roundActive)
        {
            timer.UpdateTimer();
        }
    }

    private void SpawnDinosaur()
    {
        Dinosaur dino = GameObject.Instantiate<GameObject>(DinoPrefab).GetComponent<Dinosaur>();
        dino.EntryPoint = GetPosInsideUnitCircle();
        dino.WaitForFoodPoint = ServiceZone.position;
        dino.ExitPoint = dino.WaitForFoodPoint + GetPosInsideUnitCircle();
    }

    private Vector3 GetPosInsideUnitCircle()
    {
        float randomRadius = UnityEngine.Random.Range(spawnRadius, spawnRadius + 20.0f);
        Vector2 insideCicle = (UnityEngine.Random.insideUnitCircle * randomRadius).normalized * randomRadius;
        Vector3 spawnPoint = new Vector3(insideCicle.x, 0.5f, insideCicle.y);

        return spawnPoint;
    }
}
