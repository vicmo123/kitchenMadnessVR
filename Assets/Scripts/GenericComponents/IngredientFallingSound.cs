using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientFallingSound : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.FoodDroped?.Invoke();
    }
}
