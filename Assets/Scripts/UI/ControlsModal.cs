using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsModal : MonoBehaviour, IModal {

  private Action onClose;

  public void Open() {
    gameObject.SetActive(true);
    PlayerInput.Controls.UI.Cancel.performed += ctx => Close();
  }

  public void Open(Action onClose) {
    this.onClose = onClose;
    Open();
  }

  public void Close() {
    PlayerInput.Controls.UI.Cancel.performed -= ctx => Close();
    gameObject.SetActive(false);
    onClose?.Invoke();
  }
}
