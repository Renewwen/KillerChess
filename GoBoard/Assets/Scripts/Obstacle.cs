using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Obstacle : MonoBehaviour
{
    BoxCollider boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(1f, 1f, 1f));
    }

}
