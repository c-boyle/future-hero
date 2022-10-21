using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureSeer : MonoBehaviour {

  [SerializeField] private TimeToggle presentTimeLine;

  [SerializeField] private TheFuture theFuture;

  [SerializeField] private Watch watch;

  [SerializeField] private AudioEffects audioEffects;

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
    audioEffects.SetEnabled(_timeVisionEnabled);
  }
}
