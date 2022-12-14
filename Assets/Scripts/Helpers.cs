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

  public static IEnumerator InvokeAfterTime(Action action, float timeToWait) {
    yield return new WaitForSeconds(timeToWait);
    action.Invoke();
  }
}
