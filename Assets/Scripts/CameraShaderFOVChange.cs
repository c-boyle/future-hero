using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using MyBox;
using System;

public class CameraShaderFOVChange : MonoBehaviour {

    [SerializeField] [MustBeAssigned] private Camera cam;

    [SerializeField] [ReadOnly] private bool isEffectEnabled = false;
    private float initialFOV;
    private float deltaFOV = 8;
    private float finalFOV;
    private Coroutine currentFOVCoroutine;

    // Constants
    private const float FOV_TRANSITION_MULTIPLIER = 0.25f;

    void Start() {
        initialFOV = cam.fieldOfView;
        finalFOV = initialFOV + deltaFOV;
        cam.cullingMask = cam.cullingMask & ~(1 << LayerMask.NameToLayer("FPS")); // don't render the FPS layer
    }

    void OnApplicationQuit() {
        // This has to be here because of a Unity bug or else the propery value will not go back to default
        Shader.SetGlobalFloat("_Progress", 0);
    }

    public void SetEffectEnabled(bool enabled, float transitionTime, Action onComplete = null) {
        if (isEffectEnabled == enabled) {
            return;
        }

        if (currentFOVCoroutine != null) StopCoroutine(currentFOVCoroutine);
        OnSetEffectActive(enabled, transitionTime, onComplete);
        isEffectEnabled = enabled;
    }

    private void OnSetEffectActive(bool active, float transitionTime, Action onComplete = null) {
        float fovEnd = active ? finalFOV : initialFOV;
        currentFOVCoroutine = StartCoroutine(TransitionFOV(fovEnd, transitionTime * FOV_TRANSITION_MULTIPLIER));
    }

    IEnumerator TransitionFOV(float end, float duration) {
        float elapsed_time = 0;
        float currentProgress = cam.fieldOfView;

        while (elapsed_time <= duration) {
            cam.fieldOfView = Mathf.Lerp(currentProgress, end, elapsed_time / duration);

            yield
            return null; //Waits/skips one frame

            elapsed_time += Time.deltaTime;
        }

        cam.fieldOfView = end;
    }

}