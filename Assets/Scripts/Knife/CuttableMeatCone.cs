using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttableMeatCone : MonoBehaviour, InterFace_Cutter
{
    GameObject meat;
    Vector3 posi;
    public float moveSpeed = 0.1f;
    bool hasBeenTouched = false;

    private void Start()
    {
        meat = Resources.Load<GameObject>("Prefabs/KitchenLayoutPrefabs/meatCooked");

    }

    public void Cut(RaycastHit hit)
    {
        if (!hasBeenTouched)
        {
            Instantiate(meat, hit.point, Quaternion.identity);
            hasBeenTouched = true;
            SoundManager.Slicing?.Invoke();
        }
    }


    public void StopCut()
    {
        hasBeenTouched = false;
    }
}
