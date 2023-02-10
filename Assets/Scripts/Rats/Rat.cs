using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using UnityEngine.AI;


public class Rat : MonoBehaviour
{
    #region AiData
    [HideInInspector] public NavMeshAgent agent;
    [Range(0, 100)] public float walkSpeed;
    [Range(0, 100)] public float chaseSpeed;
    [Range(0, 100)] public float scaredSpeed;
    [Range(1, 500)] public float walkRadius;
    [Range(1, 20)] public float sightLenght;
    [Range(1, 20)] public float sightRadius;
    [HideInInspector] public bool isScared = false;
    [HideInInspector] public bool objectPickedUp = false;
    [HideInInspector] public bool isBored = false;
    [HideInInspector] public int layerMask;
    [HideInInspector] public RatsManager ratManager { get; set; } = null;
    #endregion

    private RatStateMachine ratStateMachine;

    private void Awake()
    {
        ratStateMachine = new RatStateMachine(this, 10.0f);
        layerMask = LayerMask.GetMask("Food");
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = walkSpeed;
            agent.SetDestination(GenerateRandomNavMeshPos());
        }
    }

    private void Start()
    {
        ratStateMachine.InitStateMachine();
    }

    private void Update()
    {
        ratStateMachine.UpdateStateMachine();
    }

    public Vector3 GenerateRandomNavMeshPos()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, walkRadius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public bool CheckIfDestinationReached()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
            agent.SetDestination(FindClosestExit());
            objectPickedUp = true;
        }
    }

    public Vector3 FindClosestExit()
    {
        Transform bestExit = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Spawner exit in ratManager.RatHoles)
        {
            Vector3 directionToTarget = exit.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestExit = exit.transform;
            }
        }
        return bestExit.position;
    }
}
