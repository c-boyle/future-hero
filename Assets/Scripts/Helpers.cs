using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public static class Helpers {
  public static void ResetScene() {
    var activeScene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(activeScene.name);
  }

  public static void InvokeAfterTime(Action action, float timeToWait) {
    CoroutineManager.Instance.ScheduleCoroutine(_InvokeAfterTime(action, timeToWait));
  }

  private static IEnumerator _InvokeAfterTime(Action action, float timeToWait) {
    yield return new WaitForSeconds(timeToWait);
    action.Invoke();
  }
}
