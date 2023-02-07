using UnityEngine;

public class Disposer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent(out Disposable disposable);
        if (disposable)
        {
            disposable.Dispose();
        }

        other.gameObject.TryGetComponent(out Respawnable respawnable);
        if (respawnable)
        {
            respawnable.InvokeRespawn();
        }
    }
}
