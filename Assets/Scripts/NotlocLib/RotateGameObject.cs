using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGameObject : MonoBehaviour
{
    [SerializeField] Vector3 eulerRotation = Vector3.zero;
    [SerializeField] float rotationSpeed = 1f;

    void Update()
    {
        transform.Rotate(eulerRotation * rotationSpeed * Time.deltaTime);
    }
}
