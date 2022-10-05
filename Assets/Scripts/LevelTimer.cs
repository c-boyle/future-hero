using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour {
  [field: SerializeField] public float SecondsLeft { get; private set; } = 60f;

  public static event EventHandler LevelWon;

  // Update is called once per frame
  void Update() {
    SecondsLeft -= Time.deltaTime;
    if (SecondsLeft <= 0) {
      Helpers.ResetScene();
    }
  }

  public void SetSecondsLeft(float secondsLeft) {
    SecondsLeft = secondsLeft;
  }

  public void AddSecondsLeft(float seconds) {
    SecondsLeft += seconds;
  }

  public void SetLevelWon(bool won) {
    if (won) {
      LevelWon?.Invoke(this, new());
      SecondsLeft = Mathf.Infinity;
    }
  }
}
