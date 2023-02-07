using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Respawnable : MonoBehaviour
{
    [SerializeField] private Transform spawnArea;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public void InvokeRespawn()
    {
        StartCoroutine("Respawn");
    }

    IEnumerator Respawn()
    {
        meshRenderer.enabled = false;
        gameObject.transform.position = spawnArea.position;
        yield return new WaitForSeconds(3.0f);
        meshRenderer.enabled = true;
        transform.position = spawnArea.position;
    }
}
