using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using MyBox;

public class CameraShader : MonoBehaviour {

    [SerializeField] [MustBeAssigned] private PostProcessVolume volume;
    [SerializeField] [MustBeAssigned] private Camera cam;
    private bool isEffectEnabled = false;
    private float currentVolumeWeight = 0;
    private float currentCameraFOV;
    private float initialCameraFOV;
    private float finalCameraFOV;
    private const float DeltaFOV = 3;
    private const float volumeTransitionSpeed = 0.01f;
    private const float cameraFOVTransitionSpeed = 0.02f;

    void Start() {
        initialCameraFOV = cam.fieldOfView;
        finalCameraFOV = initialCameraFOV + DeltaFOV;
    }

    void Update() {
        if (isEffectEnabled) {
            currentVolumeWeight = Mathf.Lerp(currentVolumeWeight, 1, volumeTransitionSpeed);
            currentCameraFOV = Mathf.Lerp(currentCameraFOV, finalCameraFOV, cameraFOVTransitionSpeed);
        } else if (!isEffectEnabled) {
            currentVolumeWeight = Mathf.Lerp(currentVolumeWeight, 0, volumeTransitionSpeed);
            currentCameraFOV = Mathf.Lerp(currentCameraFOV, initialCameraFOV, cameraFOVTransitionSpeed);
        }

        cam.fieldOfView = currentCameraFOV;
        volume.weight = currentVolumeWeight;
    }

    public void onEffectActivate() {
    }

    public void onEffectDeactivate() {
    }

    public void ToggleShader() {
        if (!isEffectEnabled) {
            isEffectEnabled = true;
            onEffectActivate();
        }
        else {
            isEffectEnabled = false;
            onEffectDeactivate();
        }
    }
}