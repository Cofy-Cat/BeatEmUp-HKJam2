using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _vcam;
    [SerializeField] private CinemachineConfiner2D _confiner;
    [SerializeField] private string _confinerBoundTag = "CameraBound";

    private void Awake()
    {
        _vcam = GetComponentInChildren<CinemachineCamera>();
        _confiner = _vcam.GetComponent<CinemachineConfiner2D>();
    }

    private void Start()
    {
        var bound = GameObject.FindWithTag(_confinerBoundTag).GetComponent<Collider2D>();
        if (bound != null)
        {
            _confiner.BoundingShape2D = bound;
        }
    }
}
