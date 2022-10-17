using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class UIEventListener : Singleton<UIEventListener> {

  [SerializeField] private PauseMenuModal pausedGameModal;
  [SerializeField] private WinModal winModal;

  private static float timeScaleBeforePause;

  void Start() {
    LevelTimer.LevelWon += OnLevelWon;
  }

  private void OnDestroy() {
    LevelTimer.LevelWon -= OnLevelWon;
  }

  private void OnLevelWon(object sender, EventArgs e) {
    EnableUIControls();
    winModal.Open();
  }

  public void OnPausePressed() {
    PauseGame();
    EnableUIControls();
    pausedGameModal.Open(() => { UnpauseGame(); DisableUIControls(); });
  }

  public void PauseGame() {
    timeScaleBeforePause = Time.timeScale;
    Time.timeScale = 0;
  }

  public void UnpauseGame() {
    Time.timeScale = timeScaleBeforePause;
  }

  private void EnableUIControls() {
    PlayerInput.Controls.Player.Disable();
    PlayerInput.Controls.UI.Enable();
  }

  private void DisableUIControls() {
    PlayerInput.Controls.UI.Disable();
    PlayerInput.Controls.Player.Enable();
  }
}
