using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float CameraSpeed = 5;

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, CameraSpeed * Time.deltaTime);
    }

}
