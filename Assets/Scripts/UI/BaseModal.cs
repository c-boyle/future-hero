using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseModal : MonoBehaviour {

  [Tooltip("This is the button that will be selected first upon opening this modal.")][SerializeField] private Button firstSelectButton;

  private Action onClose;

  private readonly HashSet<BaseModal> openSubModals = new();

  protected virtual void OnCancel(UnityEngine.InputSystem.InputAction.CallbackContext ctx) {
    Close();
  }

  public virtual void Open() {
    if (firstSelectButton != null) {
      firstSelectButton.Select();
    }
    Show();
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
    subModal.Open(() => { Show(); openSubModals.Remove(subModal); });
    openSubModals.Add(subModal);
  }

  private void Show() {
    gameObject.SetActive(true);
    PlayerInput.Controls.UI.Cancel.performed += OnCancel;
  }

  private void Hide() {
    gameObject.SetActive(false);
    PlayerInput.Controls.UI.Cancel.performed -= OnCancel;
  }
}
