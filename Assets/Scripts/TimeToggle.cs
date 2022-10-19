using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

public class TimeToggle : MonoBehaviour {
  [Header("By default, the rendering and audio of a time toggle is disabled and enabled on time toggle enable/disable.")]
  [SerializeField] private UnityEvent timeToggleEnabled;
  [SerializeField] private UnityEvent timeToggleDisabled;

  [SerializeField][ReadOnly] private bool isHidden = true;
  [SerializeField][ReadOnly] private float alpha = 1;
  private const float alphaTransitionSpeed = 0.005f;

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
      // These functions will override any properties set using Shader.setGlobalFloat() (etc. in CameraShader).
      renderer.enabled = true;
      renderer.material.SetFloat("_isVisible", 0);
      renderer.material.SetFloat("_Progress", 1);
    }
    foreach (var audioSource in ChildAudioSources) {
      audioSource.enabled = false;
    }
  }

  void Update() {
    if (isHidden) alpha = Mathf.Lerp(alpha, 1, alphaTransitionSpeed);
    else if (!isHidden) alpha = Mathf.Lerp(alpha, 0, alphaTransitionSpeed);
    foreach (var renderer in ChildRenderers) {
      renderer.material.SetFloat("_Progress", alpha);
      if (alpha > 0.99) renderer.enabled = false;
      else renderer.enabled = true;
    }
    foreach (var audioSource in ChildAudioSources) {
      if (isHidden) audioSource.enabled = false;
      else audioSource.enabled = true;
    }
  }

  public void SetEnabled(bool enabled) {
    if (enabled && gameObject.activeSelf) {
      isHidden = false;
      timeToggleEnabled?.Invoke();
    } else {
      isHidden = true;
      timeToggleDisabled?.Invoke();
    }
  }
}
