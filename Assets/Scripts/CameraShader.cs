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
    private const float volumeTransitionSpeed = 0.01f;

    private float currentShaderProgress = 0;
    private int shaderProgressID = Shader.PropertyToID("_Progress");
    private const float shaderTransitionSpeed = 0.005f;

    private float currentCameraFOV;
    private float initialCameraFOV;
    private float finalCameraFOV;
    private const float deltaCameraFOV = 3;
    private const float cameraFOVTransitionSpeed = 0.02f;

    void Start() {
        initialCameraFOV = cam.fieldOfView;
        finalCameraFOV = initialCameraFOV + deltaCameraFOV;
        
        // This has to be here because of a Unity bug or else the propery value will not go back to default
        Shader.SetGlobalFloat(shaderProgressID, 0);
    }

    void Update() {
        if (isEffectEnabled) {
            currentVolumeWeight = Mathf.Lerp(currentVolumeWeight, 1, volumeTransitionSpeed);
            currentShaderProgress = Mathf.Lerp(currentShaderProgress, 1, shaderTransitionSpeed);
            currentCameraFOV = Mathf.Lerp(currentCameraFOV, finalCameraFOV, cameraFOVTransitionSpeed);
        } else if (!isEffectEnabled) {
            currentVolumeWeight = Mathf.Lerp(currentVolumeWeight, 0, volumeTransitionSpeed);
            currentCameraFOV = Mathf.Lerp(currentCameraFOV, initialCameraFOV, cameraFOVTransitionSpeed);
            currentShaderProgress = Mathf.Lerp(currentShaderProgress, 0, shaderTransitionSpeed);
        }

        volume.weight = currentVolumeWeight;
        Shader.SetGlobalFloat(shaderProgressID, currentShaderProgress);
        cam.fieldOfView = currentCameraFOV;
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