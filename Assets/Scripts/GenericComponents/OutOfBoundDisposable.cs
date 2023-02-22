using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundDisposable : MonoBehaviour
{
    private GameObject BoundSetter;
    private Bounds kitchenBounds;

    private void Awake()
    {
        kitchenBounds = new Bounds();
    }

    private void Start()
    {
        BoundSetter = GameObject.Find("BoundSetter");
        kitchenBounds.Encapsulate(BoundSetter.GetComponent<MeshRenderer>().bounds);
    }

    private void Update()
    {
        if (!kitchenBounds.Contains(transform.position))
        {
            gameObject.TryGetComponent(out Disposable disposable);
            if (disposable)
            {
                disposable.Dispose();
            }

            gameObject.TryGetComponent(out Respawnable respawnable);
            if (respawnable)
            {
                respawnable.InvokeRespawn();
            }
        }
    }
}
