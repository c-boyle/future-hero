using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIEventListener : Singleton<UIEventListener> {

  [SerializeField] private PauseMenuModal pausedGameModal;
  [SerializeField] private Image fadeBackground;
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
    StartCoroutine(FadeToGameEnd());
  }

  private IEnumerator FadeToGameEnd() {
    float fadeSeconds = 1f;
    float fadeStep = 1f / fadeSeconds;
    float tickTime = 0.1f;
    var waitTime = new WaitForSecondsRealtime(tickTime);
    while (fadeSeconds > 0) {
      yield return waitTime;
      var tempColor = fadeBackground.color;
      tempColor.a += fadeStep * tickTime;
      fadeBackground.color = tempColor;
      fadeSeconds -= tickTime;
    }
    SceneManager.LoadScene("GameEndMenu");
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

  public void EnableUIControls() {
    PlayerInput.Controls.Player.Disable();
    PlayerInput.Controls.UI.Enable();
    Cursor.visible = true;
    // PlayerInput.UIIsUp = true;
  }

  public void DisableUIControls() {
    PlayerInput.Controls.UI.Disable();
    PlayerInput.Controls.Player.Enable();
    Cursor.visible = false;
    // PlayerInput.UIIsUp = false;
  }

  public void InvokeActions(UnityEvent actions) {
    actions.Invoke();
  }
}
