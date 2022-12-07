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
    private Coroutine currentShaderCoroutine;
    private Coroutine currentVolumeCoroutine;

    // Constants
    private const float SHADER_TRANSITION_MULTIPLIER = 1f;
    private const float VOLUME_TRANSITION_MULTIPLIER = 0.5f;

    void Start() {
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
        if (currentVolumeCoroutine != null) StopCoroutine(currentVolumeCoroutine);
        OnSetEffectActive(enabled, transitionTime, onComplete);

        isEffectEnabled = enabled;
    }

    private void OnSetEffectActive(bool active, float transitionTime, Action onComplete = null) {
        float end = active ? 1f : 0f;
        currentShaderCoroutine = StartCoroutine(TransitionShader(end, transitionTime * SHADER_TRANSITION_MULTIPLIER, onComplete));
        currentVolumeCoroutine = StartCoroutine(TransitionVolume(end, transitionTime * VOLUME_TRANSITION_MULTIPLIER));
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