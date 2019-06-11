using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static Vector3 Vector3Round(Vector3 inputVector)
    {
        return new Vector3(Mathf.Round(inputVector.x), Mathf.Round(inputVector.y), 
                           Mathf.Round(inputVector.z));
    }

    public static Vector2 Vector2Round(Vector2 inputVector)
    {
        return new Vector2(Mathf.Round(inputVector.x), Mathf.Round(inputVector.y));
    }
}
