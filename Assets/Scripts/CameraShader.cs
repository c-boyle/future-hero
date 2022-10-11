using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraShader : MonoBehaviour {

    public PostProcessVolume volume;
    [SerializeField] private Camera cam;
    private bool isEffectEnabled = false;
    private float currentVolumeWeight = 0;
    private float volumeTransitionSpeed = 0.01f;

    void Update() {
        if (isEffectEnabled) {
            currentVolumeWeight = Mathf.Lerp(currentVolumeWeight, 1, volumeTransitionSpeed);
        } else if (!isEffectEnabled) {
            currentVolumeWeight = Mathf.Lerp(currentVolumeWeight, 0, volumeTransitionSpeed);
        }

        volume.weight = currentVolumeWeight;
    }

    public void onEffectActivate() {
        Debug.Log("activated");
    }

    public void onEffectDeactivate() {
        Debug.Log("deactivated");
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