using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class PauseMenuModal : MonoBehaviour, IModal {
  [SerializeField] private Button continueButton;
  [SerializeField] private Button controlsButton;
  [SerializeField] private Button exitButton;
  [SerializeField] private ControlsModal controlsModal;

  private void Start() {
    continueButton.onClick.AddListener(OnContinuePressed);
    controlsButton.onClick.AddListener(OnControlsPressed);
    exitButton.onClick.AddListener(OnExitPressed);
  }

  private void OnContinuePressed() {
    UIEventListener.Instance.UnpauseGame();
  }

  private void OnControlsPressed() {
    gameObject.SetActive(false);
    controlsModal.Open(() => gameObject.SetActive(true));
  }

  private void OnExitPressed() {
    Application.Quit();
  }

  public void Open() {
    gameObject.SetActive(true);
    PlayerInput.Controls.UI.Cancel.performed += ctx => Close();
  }

  public void Open(Action action) {
   
  }

  public void Close() {
    if (gameObject.activeSelf) {
      gameObject.SetActive(false);
      PlayerInput.Controls.UI.Cancel.performed -= ctx => Close();
      controlsModal.Close();
      UIEventListener.Instance.UnpauseGame();
    }
  }
}
