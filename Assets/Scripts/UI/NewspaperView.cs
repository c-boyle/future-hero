using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewspaperView : MonoBehaviour {
  [SerializeField] private TMP_Text headlineText;
  [SerializeField] private TMP_Text summaryText;
  [SerializeField] private Image picture;
  [SerializeField] private Animator animator;
  [SerializeField] private GameObject gameEndModal;
  [SerializeField] private GameEndData gameEndData;

  private float inputBlockTime;

  private void Start() {
    headlineText.text = gameEndData.Headline.ToUpper();
    var summary = gameEndData.Summary.Replace("XX:XX", System.DateTime.Now.ToString("h:mm tt"));
    summaryText.text = summary;
    picture.sprite = gameEndData.Picture;
    inputBlockTime = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
  }

  private void Update() {
    // Once the display animation is over, check for any input, then show the game end menu
    inputBlockTime -= Time.deltaTime;
    if (inputBlockTime <= 0 && Input.anyKeyDown) {
      PlayerInput.UIIsUp = true;
      gameObject.SetActive(false);
      gameEndModal.SetActive(true);
      Cursor.visible = true;
    }
  }
}
