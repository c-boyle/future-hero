using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class AltCamera : MonoBehaviour
{
    [SerializeField] [MustBeAssigned] private Camera _thisCamera;
    [SerializeField] [MustBeAssigned] private Camera _copiedCamera;
    [SerializeField] [MustBeAssigned] private LayerMask _renderLayers;
    [SerializeField] private bool _isInFront = true;
    [SerializeField] private int _depth = 0;

    void Start()
    {
        _thisCamera.CopyFrom(_copiedCamera);
        _thisCamera.clearFlags = _isInFront ? CameraClearFlags.Depth : CameraClearFlags.Nothing;
        _thisCamera.depth = _depth;
        _thisCamera.cullingMask = _renderLayers;
        _thisCamera.enabled = true;
    }

    void Update() {
        _thisCamera.fieldOfView = _copiedCamera.fieldOfView;
    }
}
