using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseModal : MonoBehaviour {

  private Action onClose;

  private readonly HashSet<BaseModal> openSubModals = new();

  private void OnCancel(UnityEngine.InputSystem.InputAction.CallbackContext ctx) {
    Close();
  }

  public virtual void Open() {
    gameObject.SetActive(true);
    PlayerInput.Controls.UI.Cancel.performed += OnCancel;
  }

  public void Open(Action onClose) {
    this.onClose = onClose;
    Open();
  }

  public virtual void Close() {
    Hide();
    onClose?.Invoke();
  }

  public void CloseChildrenModals() {
    foreach (var subModal in openSubModals) {
      subModal.CloseAll();
    }
  }

  public void CloseAll() {
    CloseChildrenModals();
    Close();
  }

  protected void OpenSubModal(BaseModal subModal) {
    Hide();
    subModal.Open(() => { Open(); openSubModals.Remove(subModal); });
    openSubModals.Add(subModal);
  }

  private void Hide() {
    gameObject.SetActive(false);
    PlayerInput.Controls.UI.Cancel.performed -= OnCancel;
  }
}
