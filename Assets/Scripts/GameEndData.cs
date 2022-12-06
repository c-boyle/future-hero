using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameEndData), menuName = nameof(GameEndData))]
public class GameEndData : ScriptableObject {
  public string Headline { get { endingsSeen.Add(CurrentEndingID); return endingIDToEndingData[CurrentEndingID].Headline; } }
  public string Summary { get { endingsSeen.Add(CurrentEndingID); return endingIDToEndingData[CurrentEndingID].Summary; } }
  public Sprite Picture { get { endingsSeen.Add(CurrentEndingID); return endingIDToEndingData[CurrentEndingID].Picture; } }
  public int TotalEndings { get => endings.Count; }
  public int EndingsSeen { get => endingsSeen.Count; }
  [field: SerializeField] public string CurrentEndingID { get; set; } = null;
  [SerializeField] private readonly HashSet<string> endingsSeen = new();
  [SerializeField] private List<SummaryData> endings = new();

  private readonly Dictionary<string, SummaryData> endingIDToEndingData = new();

  private void Awake() {
    foreach (var ending in endings) {
      endingIDToEndingData[ending.EndingID] = ending;
    }
  }

  [System.Serializable]
  private class SummaryData {
    [field: SerializeField] public string EndingID { get; set; }
    [field: SerializeField] public string Headline { get; set; }
    [field: SerializeField] public string Summary { get; set; }
    [field: SerializeField] public Sprite Picture { get; set; }
  }
}
