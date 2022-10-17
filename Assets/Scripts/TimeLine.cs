using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

public class TimeLine : MonoBehaviour {
    [Header("By default, the rendering and audio of a timeline is disabled and enabled on timeline enable/disable.")]
    [SerializeField] private UnityEvent timeLineEnabled;
    [SerializeField] private UnityEvent timeLineDisabled;

    [SerializeField] [ReadOnly] private bool isHidden = true;
    [SerializeField] [ReadOnly] private float alpha = 1;
    private const float alphaTransitionSpeed = 0.005f;

    void Start() {
        foreach (var renderer in GetComponentsInChildren<Renderer>()) {
            renderer.enabled = false;

            // These functions will override any properties set using Shader.setGlobalFloat() (etc. in CameraShader).
            renderer.enabled = true;
            renderer.material.SetFloat("_isVisible", 0);
            renderer.material.SetFloat("_Progress", 1);
        }
        foreach (var audioSource in GetComponentsInChildren<AudioSource>()) {
            audioSource.enabled = false;
        }
    }

    void Update() {
        if (isHidden) alpha = Mathf.Lerp(alpha, 1, alphaTransitionSpeed);
        else if (!isHidden) alpha = Mathf.Lerp(alpha, 0, alphaTransitionSpeed);
        foreach (var renderer in GetComponentsInChildren<Renderer>()) {
            renderer.material.SetFloat("_Progress", alpha);
            if (alpha > 0.99) renderer.enabled = false;
            else renderer.enabled = true;
        }
        foreach (var audioSource in GetComponentsInChildren<AudioSource>()) {
            if (isHidden) audioSource.enabled = false;
            else audioSource.enabled = true;
        }
    }

   public void SetEnabled(bool enabled) {
        if (enabled) {
              isHidden = false;
              timeLineEnabled?.Invoke();
        } else {
              isHidden = true;
              timeLineDisabled?.Invoke();
        }
    }
}
