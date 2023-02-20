using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttableMeatCone : MonoBehaviour, InterFace_Cutter
{
    public Transform targetPosiiton;
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
            StartCoroutine(MoveObject(meat.transform, posi, 5.0f));
            hasBeenTouched = true;
        }
    }


    public void StopCut()
    {
        hasBeenTouched = false;
    }


    IEnumerator MoveObject(Transform objectToMove, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = objectToMove.position;

        while (elapsedTime < duration)
        {
            objectToMove.position = Vector3.Lerp(startingPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectToMove.position = targetPosition;
    }

}
