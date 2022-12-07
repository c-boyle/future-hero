using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using MyBox;

public class LevelTimer : Singleton<LevelTimer> {
  [field: SerializeField] public float SecondsLeft { get; private set; } = 60f;
  private float originalStartingSeconds = 60f;

  public static event EventHandler<LevelEndEventArgs> LevelEnd;

  public static event EventHandler<TimerUpdateEventArgs> TimerUpdated;

  [SerializeField] public UnityEvent futureIsStarting;

  [SerializeField] private GameObject fireFuture;
  [SerializeField] private GameObject ruffianFuture;
  [SerializeField] private GameObject puddleFuture;
  [SerializeField] private GameEndData gameEndData;

  private bool levelEnded = false;
  private float originalSecondsLeft = -1f;
  private float setSecondsLeft;

  private bool futureNotTriggered = true;

  public static float SecondsSpentInLevel { get; private set; } = 0f;

  void Start() {
    originalStartingSeconds = SecondsLeft;
    SecondsSpentInLevel = 0f;
  }

  // Update is called once per frame
  void Update() {
    float deltaTime = Time.deltaTime;
    if (!levelEnded && !SettingsInitializer.Instance.IsTutorial) {
      SecondsLeft -= deltaTime;
      SecondsSpentInLevel += deltaTime;
      if (SecondsLeft <= 0) {
        EndLevel(false);
      } else if (GetRealSeconds() <= 10 && futureNotTriggered) {
        futureIsStarting?.Invoke();
        futureNotTriggered = false;
      }
    }
    TimerUpdated?.Invoke(this, new() { DeltaTime = deltaTime, SecondsLeft = SecondsLeft });
  }

  private float GetRealSeconds() {
    if (originalSecondsLeft == -1f) return SecondsLeft;
    return originalSecondsLeft - (setSecondsLeft - SecondsLeft);
  }

  public void LowerSecondsLeftTo(float seconds) {
    originalSecondsLeft = SecondsLeft;
    SecondsLeft = Mathf.Min(seconds, SecondsLeft);
    setSecondsLeft = SecondsLeft;
  }

  public void RestoreSecondsLeft() {
    Debug.Log("Restoring Seconds");
    if (originalSecondsLeft != -1f) {
      Debug.Log("SecondsLeft = " + originalSecondsLeft + " - (" + setSecondsLeft + " - " + SecondsLeft + ")");
      SecondsLeft = originalSecondsLeft - (setSecondsLeft - SecondsLeft);
      originalSecondsLeft = -1f;
    }
  }

  public void EndTutorial() {
    SecondsLeft = originalStartingSeconds;
    SecondsSpentInLevel = 0;
    SettingsInitializer.Instance.IsTutorial = false;
  }

  public void EndLevel(bool won) {
    if (levelEnded) return;

    // Look at the watch
    // Skip to time end
    // Stop looking at watch
    // Play end cutscene
    gameEndData.SecondsSpentInLevel = SecondsSpentInLevel;
    var endingID = string.Empty;
    endingID += fireFuture.activeSelf ? "fire" : string.Empty;
    endingID += ruffianFuture.activeSelf ? "ruffian" : string.Empty;
    endingID += puddleFuture.activeSelf ? "puddle" : string.Empty;
    gameEndData.CurrentEndingID = endingID;
    LevelEnd?.Invoke(this, new() { Won = won });
    levelEnded = true;
  }

  public class LevelEndEventArgs : EventArgs {
    [field: SerializeField] public bool Won { get; set; }
  }

  public class TimerUpdateEventArgs : EventArgs {
    [field: SerializeField] public float DeltaTime { get; set; }
    [field: SerializeField] public float SecondsLeft { get; set; }
  }
}
