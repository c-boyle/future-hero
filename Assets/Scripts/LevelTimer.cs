using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour {
  [field: SerializeField] public float SecondsLeft { get; private set; } = 60f;

  public static event EventHandler LevelWon;

  public static event EventHandler<TimerUpdateEventArgs> TimerUpdated;

  // Update is called once per frame
  void Update() {
    float deltaTime = Time.deltaTime;
    SecondsLeft -= deltaTime;
    if (SecondsLeft <= 0) {
      Helpers.ResetScene();
    }
    TimerUpdated?.Invoke(this, new() { DeltaTime = deltaTime, SecondsLeft = SecondsLeft });
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

  public class TimerUpdateEventArgs : EventArgs {
    [field: SerializeField] public float DeltaTime { get; set; }
    [field: SerializeField] public float SecondsLeft { get; set; }
  }
}
