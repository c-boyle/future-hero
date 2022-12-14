using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using MyBox;
using System;

public class CameraShader : MonoBehaviour {

    [SerializeField] [MustBeAssigned] private PostProcessVolume volume;
    [SerializeField] [MustBeAssigned] private Camera cam;

    [SerializeField] [ReadOnly] private bool isEffectEnabled = false;
    private float initialFOV;
    private float deltaFOV = 8;
    private float finalFOV;
    private Coroutine currentShaderCoroutine;
    private Coroutine currentFOVCoroutine;
    private Coroutine currentVolumeCoroutine;

    // Constants
    private const float SHADER_TRANSITION_MULTIPLIER = 1f;
    private const float VOLUME_TRANSITION_MULTIPLIER = 0.5f;
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

        if (currentShaderCoroutine != null) StopCoroutine(currentShaderCoroutine);
        if (currentVolumeCoroutine != null) StopCoroutine(currentShaderCoroutine);
        if (currentFOVCoroutine != null) StopCoroutine(currentShaderCoroutine);

        OnSetEffectActive(enabled, transitionTime, onComplete);

        isEffectEnabled = enabled;
    }

    private void OnSetEffectActive(bool active, float transitionTime, Action onComplete = null) {
        float end = active ? 1f : 0f;
        float fovEnd = active ? finalFOV : initialFOV;
        currentShaderCoroutine = StartCoroutine(TransitionShader(end, transitionTime * SHADER_TRANSITION_MULTIPLIER, onComplete));
        currentVolumeCoroutine = StartCoroutine(TransitionVolume(end, transitionTime * VOLUME_TRANSITION_MULTIPLIER));
        currentFOVCoroutine = StartCoroutine(TransitionFOV(fovEnd, transitionTime * FOV_TRANSITION_MULTIPLIER));
    }

    IEnumerator TransitionShader(float end, float duration, Action onComplete = null) {
        float elapsed_time = 0;
        float currentProgress = Shader.GetGlobalFloat("_Progress");

        while (elapsed_time <= duration) {
            Shader.SetGlobalFloat("_Progress", Mathf.Lerp(currentProgress, end, elapsed_time / duration));

            yield
            return null; //Waits/skips one frame

            elapsed_time += Time.deltaTime;
        }

        Shader.SetGlobalFloat("_Progress", end);

        onComplete?.Invoke();
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

    IEnumerator TransitionVolume(float end, float duration) {
        float elapsed_time = 0;
        float currentProgress = volume.weight;

        while (elapsed_time <= duration) {
            volume.weight = Mathf.Lerp(currentProgress, end, elapsed_time / duration);

            yield
            return null; //Waits/skips one frame

            elapsed_time += Time.deltaTime;
        }

        volume.weight = end;
    }
}