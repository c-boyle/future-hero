using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

public class TimeToggle : MonoBehaviour {
  [Header("By default, the rendering and audio of a time toggle is disabled and enabled on time toggle enable/disable.")]
  [SerializeField] private UnityEvent timeToggleEnabled;
  [SerializeField] private UnityEvent timeToggleDisabled;

  [SerializeField][ReadOnly] private bool toggleEnabled = false;

  private Dictionary<Renderer, UnityEngine.Rendering.ShadowCastingMode> rendererToShadowCastingMode = new();

  private ParticleSystem[] _cachedParticleSystems = null;
  private ParticleSystem[] ChildParticleSystems {
    get {
      if (_cachedParticleSystems == null) {
        _cachedParticleSystems = GetComponentsInChildren<ParticleSystem>();
      }
      return _cachedParticleSystems;
    }
  }

  private Renderer[] _cachedRenderers = null;
  private Renderer[] ChildRenderers {
    get {
      if (_cachedRenderers == null) {
        List<Renderer> nonParticleRenderers = new();
        foreach (var renderer in GetComponentsInChildren<Renderer>()) {
          if (renderer is not ParticleSystemRenderer) {
            nonParticleRenderers.Add(renderer);
          }
        }
        _cachedRenderers = nonParticleRenderers.ToArray();
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

  private Light[] _cachedLights = null;
  private Light[] ChildLights {
    get {
      if (_cachedLights == null) {
        _cachedLights = GetComponentsInChildren<Light>();
      }
      return _cachedLights;
    }
  }

  private void Start() {
    _cachedAudioSources = null;
    _cachedRenderers = null;
  }

  void Update() {
    // These functions will override any properties set using Shader.setGlobalFloat() (etc. in CameraShader).
    // Disable children that have faded away
    // TODO: Disable children renderer once they are entirely transparent
    float progress = Shader.GetGlobalFloat("_Progress");

    if (progress < 0.02 && !toggleEnabled)
      foreach (var renderer in ChildRenderers)
        renderer.enabled = false;

  }

#if UNITY_EDITOR
  [ButtonMethod]
  public void Toggle() {
    _cachedRenderers = GetComponentsInChildren<Renderer>();
    _cachedAudioSources = GetComponentsInChildren<AudioSource>();
    _cachedLights = GetComponentsInChildren<Light>();
    SetEnabled(!toggleEnabled);
  }
#endif

  public void SetEnabled(bool enabled) {
    if (gameObject.activeSelf) {
      this.toggleEnabled = enabled;
      foreach (var audioSource in ChildAudioSources) {
        audioSource.enabled = enabled;
      }
      foreach (var particleSystem in ChildParticleSystems) {
        if (enabled) {
          particleSystem.Play(); 
        } else {
          particleSystem.Stop();
        }
      }
      foreach (var renderer in ChildRenderers) {
        renderer.enabled = enabled;
        if (rendererToShadowCastingMode.TryGetValue(renderer, out var mode)) {
          renderer.shadowCastingMode = mode;
        }
      }
      foreach (var light in ChildLights) {
        light.enabled = enabled;
      }
      var toggleEvent = enabled ? timeToggleEnabled : timeToggleDisabled;
      toggleEvent?.Invoke();
    }
  }

  public void DisableImmediateComponents() {
    if (gameObject.activeSelf) {
      foreach (var particleSystem in ChildParticleSystems) {
        particleSystem.Stop();
      }
      foreach (var renderer in ChildRenderers) {
        rendererToShadowCastingMode[renderer] = renderer.shadowCastingMode;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
      }
      foreach (var light in ChildLights) {
        light.enabled = false;
      }
    }
  }
}
