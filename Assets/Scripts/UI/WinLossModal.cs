using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinLossModal : BaseModal {
  [SerializeField] private Button restartButton;
  [SerializeField] private Button mainMenuButton;

  private void Start() {
    restartButton.onClick.AddListener(OnRestartPressed);
    mainMenuButton.onClick.AddListener(OnMainMenuPressed);
  }

  protected override void OnCancel(InputAction.CallbackContext ctx) { }

  private void OnRestartPressed() {
    CloseAll();
    Helpers.ResetScene();
  }

  private void OnMainMenuPressed() {
    CloseAll();
    SceneManager.LoadScene("MainMenu");
  }
}
