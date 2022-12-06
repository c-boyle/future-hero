using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

public class UIEventListener : Singleton<UIEventListener> {

  [SerializeField] private PauseMenuModal pausedGameModal;
  [SerializeField] private WinLossModal winModal;
  [SerializeField] private WinLossModal lossModal;
  [SerializeField] private InteractableControlsPrompt interactableControlsPrompt;
  [SerializeField] private ControlsPromptPanel controlsPromptPanel;

  private static float timeScaleBeforePause;

  public bool GameIsPaused { get; private set; }

  void Start() {
    LevelTimer.LevelEnd += OnLevelEnd;
    PlayerInput.OnPlayerInput += OnPlayerInput;
  }

  private void OnDestroy() {
    LevelTimer.LevelEnd -= OnLevelEnd;
    PlayerInput.OnPlayerInput -= OnPlayerInput;
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

  private void OnPlayerInput(object sender, PlayerInput.PlayerInputEventArgs e) {
    controlsPromptPanel.Refresh(e);
    interactableControlsPrompt.Refresh(e);
  }

  public void OnPausePressed() {
    PauseGame();
    EnableUIControls();
    interactableControlsPrompt.Hide();
    controlsPromptPanel.Hide();
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
    interactableControlsPrompt.Show();
    controlsPromptPanel.Show();
  }

  public void ShowTimeTogglePrompt() {
    controlsPromptPanel.ShowTimeTogglePrompt();
  }

  public void HideTimeTogglePrompt() {
    controlsPromptPanel.HideTimeTogglePrompt();
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

  public void InvokeActions(UnityEvent actions) {
    actions.Invoke();
  }
}
