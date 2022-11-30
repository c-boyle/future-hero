using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableControlsPrompt : MonoBehaviour {
  [SerializeField] private GameObject interactPrompt;
  [SerializeField] private TMP_Text interactPromptText;
  [SerializeField] private GameObject pickupPrompt;

  private Color? originalInteractColor = null;

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
      bool meetsItemRequirement = e.InRangeInteractable.MeetsItemRequirement(e.ItemHolder);
      var interactText =  e.InRangeInteractable.promptText;
      interactText += meetsItemRequirement ? "" : $" [Item Needed: {e.InRangeInteractable.RequiredItem.ItemName}]";
      interactPromptText.text = interactText;
      pickupPrompt.SetActive(e.InRangeInteractable.IsItem);
      if (!originalInteractColor.HasValue) {
        originalInteractColor = interactPromptText.color;
      }
      interactPromptText.color = meetsItemRequirement ? originalInteractColor.Value : Color.red;
    } else {
      Hide();
    }
  }
}
