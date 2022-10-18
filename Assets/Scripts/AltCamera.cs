using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class AltCamera : MonoBehaviour
{
    [SerializeField] [MustBeAssigned] private Camera _thisCamera;
    [SerializeField] [MustBeAssigned] private Camera _copiedCamera;
    [SerializeField] [MustBeAssigned] private LayerMask _renderLayers;
    [SerializeField] [MustBeAssigned] private int _depth;

    void Start()
    {
        _thisCamera.CopyFrom(_copiedCamera);
        _thisCamera.clearFlags = CameraClearFlags.Depth;
        _thisCamera.depth = _depth;
        _thisCamera.cullingMask = _renderLayers;
        _thisCamera.enabled = true;
    }
}
