using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionFunctions
{
    public static int GetRandomElement<T>(this T[] list)
    {
        return Random.Range(0, list.Length);
    }
}
