using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeLine : MonoBehaviour {
  [Header("By default, the rendering of a timeline is disabled and enabled on timeline enable/disable.")]
  [SerializeField] private UnityEvent timeLineEnabled;
  [SerializeField] private UnityEvent timeLineDisabled;

  public void SetEnabled(bool enabled) {
    foreach (var renderer in GetComponentsInChildren<Renderer>()) {
      renderer.enabled = enabled;
    }
    if (enabled) {
      timeLineEnabled?.Invoke();
    } else {
      timeLineDisabled?.Invoke();
    }
  }
}
