using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class UIEventListener : Singleton<UIEventListener> {

  [SerializeField] private PauseMenuModal pausedGameModal;

  private static float timeScaleBeforePause;

  public void PauseGame() {
    PlayerInput.Controls.Player.Disable();
    PlayerInput.Controls.UI.Enable();
    timeScaleBeforePause = Time.timeScale;
    Time.timeScale = 0;
    pausedGameModal.Open();
  }

  public void UnpauseGame() {
    Time.timeScale = timeScaleBeforePause;
    pausedGameModal.Close();
    PlayerInput.Controls.UI.Disable();
    PlayerInput.Controls.Player.Enable();
  }
}
