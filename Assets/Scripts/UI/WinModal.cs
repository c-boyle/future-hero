using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinModal : BaseModal {
  [SerializeField] private Button playAgainButton;
  [SerializeField] private Button mainMenuButton;

  private void Start() {
    playAgainButton.onClick.AddListener(OnPlayAgainPressed);
    mainMenuButton.onClick.AddListener(OnMainMenuPressed);
  }

  protected override void OnCancel(InputAction.CallbackContext ctx) { }

  private void OnPlayAgainPressed() {
    CloseAll();
    Helpers.ResetScene();
  }

  private void OnMainMenuPressed() {
    CloseAll();
    SceneManager.LoadScene("MainMenu");
  }
}
