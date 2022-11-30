using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableControlsPrompt : MonoBehaviour {
  [SerializeField] private TMP_Text interactPromptText;

  public void Show() {
    gameObject.SetActive(true);
  }

  public void Hide() {
    gameObject.SetActive(false);
  }

  public void Refresh(PlayerInput.PlayerInputEventArgs e) {
    if (e.InRangeInteractable != null) {
      Show();
      interactPromptText.text = e.InRangeInteractable.promptText;
    } else {
      Hide();
    }
  }
}
