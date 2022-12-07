using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndModal : MonoBehaviour {
  [field: SerializeField] private TMP_Text timeText;
  [field: SerializeField] private TMP_Text outOfText;
  [field: SerializeField] private Button playAgainButton;
  [field: SerializeField] private Button mainMenuButton;
  [field: SerializeField] private GameEndData gameEndData;

  private const string OUT_OF = " Out Of ";

  // Start is called before the first frame update
  void Start() {
    var completionTime = new TimeSpan(0, 0, (int)gameEndData.SecondsSpentInLevel);
    timeText.text = completionTime.ToString("g").Substring(3);
    outOfText.text = gameEndData.EndingsSeen + OUT_OF + gameEndData.TotalEndings;
    playAgainButton.onClick.AddListener(OnPlayAgainPressed);
    mainMenuButton.onClick.AddListener(OnMainMenuPressed);
  }

  private void OnPlayAgainPressed() {
   SceneManager.LoadScene("Level1");
  }

  private void OnMainMenuPressed() {
    SceneManager.LoadScene("MainMenu");
  }
}
