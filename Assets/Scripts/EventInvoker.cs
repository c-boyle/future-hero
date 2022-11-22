using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventInvoker : MonoBehaviour {
  [SerializeField] private UnityEvent unityEvent;
  [SerializeField] private bool invokeOnStart = false;

  private void Start() {
    if (invokeOnStart) {
      unityEvent?.Invoke();
    }
  }

  public void InvokeAfterDelay(float seconds) {
    Helpers.InvokeAfterTime(() => unityEvent.Invoke(), seconds);
  }
}
