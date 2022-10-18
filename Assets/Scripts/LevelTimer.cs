using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class LevelTimer : Singleton<LevelTimer> {
  [field: SerializeField] public float SecondsLeft { get; private set; } = 60f;

  public static event EventHandler<LevelEndEventArgs> LevelEnd;

  public static event EventHandler<TimerUpdateEventArgs> TimerUpdated;

  // Update is called once per frame
  void Update() {
    float deltaTime = Time.deltaTime;
    SecondsLeft -= deltaTime;
    if (SecondsLeft <= 0) {
      EndLevel(false);
    }
    TimerUpdated?.Invoke(this, new() { DeltaTime = deltaTime, SecondsLeft = SecondsLeft });
  }

  public void SetSecondsLeft(float secondsLeft) {
    SecondsLeft = secondsLeft;
  }

  public void AddSecondsLeft(float seconds) {
    SecondsLeft += seconds;
  }

  public void EndLevel(bool won) {
    LevelEnd?.Invoke(this, new() { Won = won });
    SecondsLeft = Mathf.Infinity;
  }

  public class LevelEndEventArgs : EventArgs {
    [field: SerializeField] public bool Won { get; set; }
  }

  public class TimerUpdateEventArgs : EventArgs {
    [field: SerializeField] public float DeltaTime { get; set; }
    [field: SerializeField] public float SecondsLeft { get; set; }
  }
}
