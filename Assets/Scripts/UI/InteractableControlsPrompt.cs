using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableControlsPrompt : MonoBehaviour {
  [SerializeField] private GameObject interactPrompt;
  [SerializeField] private TMP_Text interactPromptText;
  [SerializeField] private GameObject pickupPrompt;
  [SerializeField] private TMP_Text pickupPromptText;

  private const string PICKUP_TEXT = "Pick Up";
  private const string PICKUP_TEXT_ITEM_HELD = "Pick Up [Must Drop Item First]";

  public void Show() {
    gameObject.SetActive(true);
  }

  public void Hide() {
    gameObject.SetActive(false);
  }

  public void Refresh(PlayerInput.PlayerInputEventArgs e) {
    if (e.TimeVisionEnabled) {
      Hide();
      return;
    }
    if (e.InRangeInteractable != null) {
      Show();
      if (e.InRangeInteractable.Item == null) {
        pickupPrompt.SetActive(false);
        interactPrompt.SetActive(true);
        bool meetsItemRequirement = e.InRangeInteractable.MeetsItemRequirement(e.ItemHolder);
        var interactText = e.InRangeInteractable.promptText;
        interactText += meetsItemRequirement ? "" : " [Item Needed]";
        interactPromptText.text = interactText;
      } else {
        interactPrompt.SetActive(false);
        pickupPrompt.SetActive(true);
        pickupPromptText.text = e.ItemHolder.holding ? PICKUP_TEXT_ITEM_HELD : PICKUP_TEXT;
      }
      if (e.InRangeInteractable.Item is Cup && e.ItemHolder.HeldItem is Cigarette cig && cig.smoking) {
        interactPrompt.SetActive(true);
        interactPromptText.text = "Extinguish";
      }
    } else {
      Hide();
    }
  }
}
