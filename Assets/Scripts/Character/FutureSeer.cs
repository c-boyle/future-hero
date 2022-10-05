using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureSeer : MonoBehaviour {

  [SerializeField] private GameObject presentObjectsRoot;

  [SerializeField] private GameObject futureObjectsRoot;

  [SerializeField] private bool _timeVisionEnabled = false;

  public bool TimeVisionEnabled { get => _timeVisionEnabled; }

  public void ToggleFutureVision() {
    _timeVisionEnabled = !_timeVisionEnabled;
    foreach (var renderer in futureObjectsRoot.GetComponentsInChildren<Renderer>()) {
      renderer.enabled = _timeVisionEnabled;
    }
    foreach (var renderer in presentObjectsRoot.GetComponentsInChildren<Renderer>()) {
      renderer.enabled = !_timeVisionEnabled;
    }
  }

  public void SetFutureScene(GameObject futureObjectsRoot) {
    if (_timeVisionEnabled) {
      foreach (var renderer in this.futureObjectsRoot.GetComponentsInChildren<Renderer>()) {
        renderer.enabled = false;
      }
      foreach (var renderer in futureObjectsRoot.GetComponentsInChildren<Renderer>()) {
        renderer.enabled = true;
      }
    }
    this.futureObjectsRoot = futureObjectsRoot;
  }
}
