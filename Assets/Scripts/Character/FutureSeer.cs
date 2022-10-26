using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FutureSeer : MonoBehaviour {

  [SerializeField] private TimeToggle presentTimeLine;

  [SerializeField] private TheFuture theFuture;

  [SerializeField] private Watch watch;

  [SerializeField] private float transitionSeconds = 1f;

  [SerializeField] private FutureAudio futureAudio;

  [SerializeField] private IntroText introText;

  [SerializeField] private bool _timeVisionEnabled = false;

  public bool TimeVisionEnabled { get => _timeVisionEnabled; }

  private void Start() {
    presentTimeLine.SetEnabled(true);
    theFuture.SetEnabled(false);
  }

  public void ToggleFutureVision() {
    _timeVisionEnabled = !_timeVisionEnabled;
    presentTimeLine.SetEnabled(!_timeVisionEnabled);
    theFuture.SetEnabled(_timeVisionEnabled);
    watch.toggleFutureTime(_timeVisionEnabled);
    futureAudio.SetFutureAudio(_timeVisionEnabled, transitionSeconds);

    if (introText) {
      introText.StartGame();
    }
  }

  [System.Serializable]
  private class FutureAudio {
    [SerializeField] private AudioMixer audioMixer;

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
    }
  }
}
