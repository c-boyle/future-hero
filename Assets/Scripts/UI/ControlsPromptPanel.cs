using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsPromptPanel : MonoBehaviour {
  [SerializeField] private GameObject dropPrompt;
  [SerializeField] private GameObject throwPrompt;
  [SerializeField] private GameObject toggleTimePrompt;
  [SerializeField] private TMP_Text futureText;
  [SerializeField] private TMP_Text presentText;

  public void Show() {
    gameObject.SetActive(true);
  }

  public void Hide() {
    gameObject.SetActive(false);
  }

  public void Refresh(PlayerInput.PlayerInputEventArgs e) {
    dropPrompt.SetActive(e.ItemHolder.holding);
    throwPrompt.SetActive(e.ItemHolder.holding);
    futureText.gameObject.SetActive(!e.TimeVisionEnabled);
    presentText.gameObject.SetActive(e.TimeVisionEnabled);
  }

  public void ShowTimeTogglePrompt() {
    toggleTimePrompt.SetActive(true);
  }

  public void HideTimeTogglePrompt() {
    toggleTimePrompt.SetActive(false);
  }
}
