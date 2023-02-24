using UnityEngine;

public class Disposable : MonoBehaviour
{
    public void Dispose()
    {
        Destroy(gameObject);
        SoundManager.FoodFallsInTrash?.Invoke();
    }
}
