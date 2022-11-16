using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventInvoker : MonoBehaviour {
  [SerializeField] private UnityEvent unityEvent;

  public void InvokeAfterDelay(float seconds) {
    Helpers.InvokeAfterTime(() => unityEvent.Invoke(), seconds);
  }
}
