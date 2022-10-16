using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureSeer : MonoBehaviour {

  [SerializeField] private TimeLine presentTimeLine;

  [SerializeField] private TimeLine futureTimeLine;

  [SerializeField] private Watch watch;

  [SerializeField] private bool _timeVisionEnabled = false;

  public bool TimeVisionEnabled { get => _timeVisionEnabled; }

  public void ToggleFutureVision() {
    _timeVisionEnabled = !_timeVisionEnabled;
    presentTimeLine.SetEnabled(!_timeVisionEnabled);
    futureTimeLine.SetEnabled(_timeVisionEnabled);
    watch.toggleFutureTime(_timeVisionEnabled);
  }

  public void SetFuture(TimeLine futureTimeLine) {
    if (_timeVisionEnabled) {
      this.futureTimeLine.SetEnabled(false);
      futureTimeLine.SetEnabled(true);
    }
    this.futureTimeLine = futureTimeLine;
  }
}
