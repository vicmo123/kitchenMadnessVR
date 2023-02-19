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
    //Max speed
    [Range(0, 100)] public float scaredSpeed;
    [Range(0, 100)] public float walkRadius;
    [Range(1, 20)] public float sightLenght;
    [Range(1, 20)] public float sightRadius;
    [HideInInspector] public bool isHit = false;
    [HideInInspector] public bool objectPickedUp = false;
    [HideInInspector] public bool isBored = false;
    [HideInInspector] public int layerMask;
    [HideInInspector] public int areaMask;
    public bool isGrabbed { get; set; }
    [HideInInspector] public RatsManager ratManager { get; set; } = null;
    #endregion

    private RatStateMachine ratStateMachine;
    [HideInInspector] public Animator animCtrl { get; private set; }


    private void Awake()
    {
        ratStateMachine = new RatStateMachine(this, 10.0f);
        layerMask = LayerMask.GetMask("Food");
        animCtrl = gameObject.GetComponentInChildren<Animator>();

        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            areaMask = agent.areaMask;
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
        float currentSpeed = agent.velocity.magnitude;
        currentSpeed = Mathf.Clamp(currentSpeed * 2.0f, 0, chaseSpeed);
        animCtrl.SetFloat("Speed", currentSpeed / chaseSpeed);

        SeeIfGrabbedByPlayer();
    }

    public Vector3 GenerateRandomNavMeshPos()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere.normalized * walkRadius;
        randomDirection += transform.position;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, walkRadius + transform.position.y, areaMask))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public bool CheckIfDestinationReached()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }
        return false;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            agent.SetDestination(FindClosestExit());
            objectPickedUp = true;
            animCtrl.SetBool("IsItemPickedUp", objectPickedUp);
        }

        if (other.gameObject.CompareTag("Right Hand") || other.gameObject.CompareTag("Left Hand"))
        {
            isHit = true;
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

    public void SeeIfGrabbedByPlayer()
    {
        if (isGrabbed)
        {
            agent.enabled = false;
        }
        if (!isGrabbed)
        {
            agent.enabled = true;
        }
    }
}
