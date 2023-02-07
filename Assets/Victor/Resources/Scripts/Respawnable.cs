using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Respawnable : MonoBehaviour
{
    public Action OnRespawnLogic = () => { };
    [SerializeField] private Transform spawLocationEditor;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public void InvokeRespawn()
    {
        StartCoroutine(Respawn());
        OnRespawnLogic.Invoke();
    }

    public void InvokeRespawn(Transform spawnLocation, float timeBeforeRespawn)
    {
        StartCoroutine(Respawn(spawnLocation, timeBeforeRespawn));
        OnRespawnLogic.Invoke();
    }

    private IEnumerator Respawn(Transform spawnLocation = null, float timeBeforeRespawn = 0.5f)
    {
        meshRenderer.enabled = false;
        gameObject.transform.position = spawnLocation != null ? spawnLocation.position : spawLocationEditor.position;
        yield return new WaitForSeconds(timeBeforeRespawn);
        meshRenderer.enabled = true;
        transform.position = spawnLocation != null ? spawnLocation.position : spawLocationEditor.position;
    }
}
