using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

public class TimeToggle : MonoBehaviour {
     [Header("By default, the rendering and audio of a time toggle is disabled and enabled on time toggle enable/disable.")]
    [SerializeField] private UnityEvent timeToggleEnabled;
    [SerializeField] private UnityEvent timeToggleDisabled;

    [SerializeField] [ReadOnly] private bool isFuture = false;

    private Renderer[] _cachedRenderers = null;
    private Renderer[] ChildRenderers {
        get {
            if (_cachedRenderers == null) {
                _cachedRenderers = GetComponentsInChildren<Renderer>();
            }
            return _cachedRenderers;
        }
    }

    private AudioSource[] _cachedAudioSources = null;
    private AudioSource[] ChildAudioSources {
        get {
            if (_cachedAudioSources == null) {
            _cachedAudioSources = GetComponentsInChildren<AudioSource>();
            }
            return _cachedAudioSources;
        }
    }

    void Start() {
        foreach (var renderer in ChildRenderers) {
            renderer.enabled = false;
        }
        foreach (var audioSource in ChildAudioSources) {
            audioSource.enabled = false;
        }
    }

    void Update() {
        // These functions will override any properties set using Shader.setGlobalFloat() (etc. in CameraShader).
        // Disable children that have faded away
        // Children are expected to have _BlendFrom set as TRANSPARENT
        float progress = Shader.GetGlobalFloat("_Progress");
        if (progress < 0.1 && !isFuture)
            foreach (var renderer in ChildRenderers)
                renderer.enabled = false;
    }

    public void SetEnabled(bool enabled) {
        if (enabled && gameObject.activeSelf) {
            isFuture = true;
            timeToggleEnabled?.Invoke();
            foreach (var audioSource in ChildAudioSources) {
                audioSource.enabled = true;
            }
            foreach (var renderer in ChildRenderers) {
                renderer.enabled = true;
            }
        }
        else {
            isFuture = false;
            timeToggleDisabled?.Invoke();
            foreach (var audioSource in ChildAudioSources)
                audioSource.enabled = false;
        }
    }
}
