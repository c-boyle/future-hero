using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Helpers {
  public static void ResetScene() {
    var activeScene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(activeScene.name);
  }
}
