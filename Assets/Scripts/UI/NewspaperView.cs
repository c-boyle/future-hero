using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewspaperView : MonoBehaviour {
  [SerializeField] private TMP_Text headlineText;
  [SerializeField] private TMP_Text summaryText;
  [SerializeField] private Image picture;
  [SerializeField] private GameEndData gameEndData;

  private void Start() {
    headlineText.text = gameEndData.Headline;
    summaryText.text = gameEndData.Summary;
    picture.sprite = gameEndData.Picture;
    // Begin display animation
  }

  private void Update() {
    // Once the display animation is over, check for any input, then show the game end menu
  }
}
