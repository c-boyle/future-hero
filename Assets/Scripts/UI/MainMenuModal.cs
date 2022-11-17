using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuModal : BaseModal {

  [SerializeField] private Button playButton;
  [SerializeField] private Button controlsButton;
  [SerializeField] private BaseModal controlsModal;

  // Start is called before the first frame update
  void Start() {
    playButton.onClick.AddListener(OnPlayPressed);
    controlsButton.onClick.AddListener(OnControlsPressed);
    PlayerInput.Controls = new();
    PlayerInput.Controls.UI.Enable();
  }

  protected override void OnCancel(UnityEngine.InputSystem.InputAction.CallbackContext ctx) { }

  private void OnControlsPressed() {
    OpenSubModal(controlsModal);
  }

  private void OnPlayPressed() {
    CloseAll();
    PlayerInput.Controls = null;
    SceneManager.LoadScene("Level1");
  }
}
