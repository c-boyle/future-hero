using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsPromptPanel : MonoBehaviour {
  [SerializeField] private GameObject dropPrompt;
  [SerializeField] private GameObject throwPrompt;
  [SerializeField] private GameObject toggleTimePrompt;
  [SerializeField] private TMP_Text toggleTimePromptText;

  public void Show() {
    gameObject.SetActive(true);
  }

  public void Hide() {
    gameObject.SetActive(false);
  }

  public void Refresh(PlayerInput.PlayerInputEventArgs e) {
    dropPrompt.SetActive(e.HoldingItem);
    throwPrompt.SetActive(e.HoldingItem);
    string timeText = "Look Into " + (e.TimeVisionEnabled ? "Past" : "Future");
    toggleTimePromptText.text = timeText;
  }
}
