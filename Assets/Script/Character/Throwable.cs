using System;
using UnityEngine;

public class Throwable: MonoBehaviour
{
    [SerializeField] private bool isAttached;

    private Transform _attachGroundPoint;

    private void Update()
    {
        if (isAttached)
        {
            transform.position = _attachGroundPoint.position;
        }
    }

    public void AttachToTransform(Transform attachPoint)
    {
        _attachGroundPoint = attachPoint;
        isAttached = true;
    }
}