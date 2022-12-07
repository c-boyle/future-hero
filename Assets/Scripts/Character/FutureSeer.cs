using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FutureSeer : MonoBehaviour {

  [SerializeField] private TimeToggle presentTimeLine;
  [SerializeField] private TheFuture theFuture;

  [SerializeField] private Watch watch;

  [SerializeField] private UIText introText;

  [SerializeField] private float transitionSeconds = 1f;
  [SerializeField] private FutureAudio futureAudio;
  [SerializeField] private CameraShader futureShader;
  [SerializeField] private CameraShaderFOVChange futureShaderFOVChange;

  [SerializeField] private bool _timeVisionEnabled = false;

  public bool TimeVisionEnabled { get => _timeVisionEnabled; }

  private void Start() {
    presentTimeLine.SetEnabled(true);
    theFuture.SetEnabled(false);
  }

  public void ToggleFutureVision() {
    _timeVisionEnabled = !_timeVisionEnabled;
    watch.toggleFutureTime(_timeVisionEnabled);
    futureAudio.SetFutureAudio(_timeVisionEnabled, transitionSeconds);

    Action disableTimeline;
    if (_timeVisionEnabled) {
      theFuture.SetEnabled(_timeVisionEnabled);
      presentTimeLine.DisableImmediateComponents();
      disableTimeline = () => presentTimeLine.SetEnabled(!_timeVisionEnabled);
    } else {
      presentTimeLine.SetEnabled(!_timeVisionEnabled);
      theFuture.DisableImmediateComponents();
      disableTimeline = () => theFuture.SetEnabled(_timeVisionEnabled);
    }

    futureShader.SetEffectEnabled(_timeVisionEnabled, transitionSeconds, disableTimeline);
    futureShaderFOVChange.SetEffectEnabled(_timeVisionEnabled, transitionSeconds, disableTimeline);

        // if (introText) {
        //   introText.StartGame();
        // }
    }

  [System.Serializable]
  private class FutureAudio {
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource transitionSound;

    private const string FUTURE = "Future";
    private const string PRESENT = "Present";

    private AudioMixerSnapshot _presentSnapshot = null;
    private AudioMixerSnapshot _futureSnapshot = null;

    private AudioMixerSnapshot PresentSnapshot {
      get {
        if (_presentSnapshot == null) {
          _presentSnapshot = audioMixer.FindSnapshot(PRESENT);
        }
        return _presentSnapshot;
      }
    }

    private AudioMixerSnapshot FutureSnapshot {
      get {
        if (_futureSnapshot == null) {
          _futureSnapshot = audioMixer.FindSnapshot(FUTURE);
        }
        return _futureSnapshot;
      }
    }

    public void SetFutureAudio(bool futureEnabled, float transitionTime) {
      if (futureEnabled) {
        FutureSnapshot.TransitionTo(transitionTime);
      } else {
        PresentSnapshot.TransitionTo(transitionTime);
      }
      transitionSound.pitch = transitionSound.clip.length / transitionTime;
      transitionSound.Play();
    }
  }
}
