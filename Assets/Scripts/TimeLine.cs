using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

public class TimeLine : MonoBehaviour {
    [Header("By default, the rendering and audio of a timeline is disabled and enabled on timeline enable/disable.")]
    [SerializeField] private UnityEvent timeLineEnabled;
    [SerializeField] private UnityEvent timeLineDisabled;
    [SerializeField] [ReadOnly] private bool isFuture = false;

    void Start() {
        foreach (var renderer in GetComponentsInChildren<Renderer>()) {
            renderer.enabled = false;
        }
        foreach (var audioSource in GetComponentsInChildren<AudioSource>()) {
            audioSource.enabled = false;
        }
    }

    void Update() {
        // Disable children that have faded away
        // Children are expected to have _BlendFrom set as TRANSPARENT
        float progress = Shader.GetGlobalFloat("_Progress");
        if (progress < 0.1 && !isFuture)
            foreach (var renderer in GetComponentsInChildren<Renderer>())
                renderer.enabled = false;
    }

   public void SetEnabled(bool enabled) {
        if (enabled) {
            isFuture = true;
            timeLineEnabled?.Invoke();
            foreach (var audioSource in GetComponentsInChildren<AudioSource>()) {
                audioSource.enabled = true;
            }
            foreach (var renderer in GetComponentsInChildren<Renderer>()) {
                renderer.enabled = true;
            }
        } else {
            isFuture = false;
            timeLineDisabled?.Invoke();
            foreach (var audioSource in GetComponentsInChildren<AudioSource>())
                audioSource.enabled = false;
        }
    }
}
