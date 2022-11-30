using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableControlsPrompt : MonoBehaviour {
  [SerializeField] private GameObject interactPrompt;
  [SerializeField] private TMP_Text interactPromptText;
  [SerializeField] private GameObject pickupPrompt;

  public void Show() {
    gameObject.SetActive(true);
  }

  public void Hide() {
    gameObject.SetActive(false);
  }

  public void Refresh(PlayerInput.PlayerInputEventArgs e) {
    if (e.InRangeInteractable != null) {
      Show();
      interactPrompt.SetActive(true);
      interactPromptText.text = e.InRangeInteractable.promptText;
      pickupPrompt.SetActive(e.InRangeInteractable.IsItem);
    } else {
      Hide();
    }
  }
}
