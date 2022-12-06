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
  [SerializeField] private Animator animator;

  private float inputBlockTime;

  private void Start() {
    headlineText.text = gameEndData.Headline.ToUpper();
    summaryText.text = gameEndData.Summary;
    picture.sprite = gameEndData.Picture;
    inputBlockTime = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
  }

  private void Update() {
    // Once the display animation is over, check for any input, then show the game end menu
    if (inputBlockTime <= 0 && Input.anyKeyDown) {
      
    }
  }
}
