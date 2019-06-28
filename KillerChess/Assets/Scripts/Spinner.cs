using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float rotateSpeed = 20f;
    // Update is called once per frame
    void Update()
    {
        iTween.RotateBy(gameObject, iTween.Hash(
            "y", 360f,
            "looptype", iTween.LoopType.loop,
            "speed", rotateSpeed,
            "easetype", iTween.EaseType.linear
            ));
    }
}
