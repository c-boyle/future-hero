using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureSeer : MonoBehaviour {

  [SerializeField] private GameObject presentObjectsRoot;

  [SerializeField] private GameObject futureObjectsRoot;

  [SerializeField] private bool _timeVisionEnabled = false;

  public bool TimeVisionEnabled { get => _timeVisionEnabled; }

  public void ToggleFutureVision() {
    if (_timeVisionEnabled) {
      futureObjectsRoot.SetActive(false);
      presentObjectsRoot.SetActive(true);
    } else {
      futureObjectsRoot.SetActive(true);
      presentObjectsRoot.SetActive(false);
    }
    _timeVisionEnabled = !_timeVisionEnabled;
  }

  public void SetFutureScene(GameObject futureObjectsRoot) {
    if (_timeVisionEnabled) {
      this.futureObjectsRoot.SetActive(false);
      futureObjectsRoot.SetActive(true);
    }
    this.futureObjectsRoot = futureObjectsRoot;
  }
}
