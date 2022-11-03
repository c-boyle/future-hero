using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class UIEventListener : Singleton<UIEventListener> {

  [SerializeField] private PauseMenuModal pausedGameModal;
  [SerializeField] private WinLossModal winModal;
  [SerializeField] private WinLossModal lossModal;

  private static float timeScaleBeforePause;

  public bool GameIsPaused { get; private set; }

  void Start() {
    LevelTimer.LevelEnd += OnLevelEnd;
  }

  private void OnDestroy() {
    LevelTimer.LevelEnd -= OnLevelEnd;
  }

  private void OnLevelEnd(object sender, LevelTimer.LevelEndEventArgs e) {
    EnableUIControls();
    if (e.Won) {
      winModal.Open(() => DisableUIControls());
    } else {
      PauseGame();
      lossModal.Open(() => { UnpauseGame(); DisableUIControls(); });
    }
  }

  public void OnPausePressed() {
    PauseGame();
    EnableUIControls();
    pausedGameModal.Open(() => { UnpauseGame(); DisableUIControls(); });
  }

  public void PauseGame() {
    timeScaleBeforePause = Time.timeScale;
    Time.timeScale = 0;
    GameIsPaused = true;
  }

  public void UnpauseGame() {
    Time.timeScale = timeScaleBeforePause;
    GameIsPaused = false;
  }

  private void EnableUIControls() {
    PlayerInput.Controls.Player.Disable();
    PlayerInput.Controls.UI.Enable();
    Cursor.visible = true;
  }

  private void DisableUIControls() {
    PlayerInput.Controls.UI.Disable();
    PlayerInput.Controls.Player.Enable();
    Cursor.visible = false;
  }
}
