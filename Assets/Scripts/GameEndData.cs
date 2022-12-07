using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameEndData), menuName = nameof(GameEndData))]
public class GameEndData : ScriptableObject {
  public string Headline { get { if (!endingsSeen.Contains(CurrentEndingID)) endingsSeen.Add(CurrentEndingID); return EndingIDToEndingData[CurrentEndingID].Headline; } }
  public string Summary { get { if (!endingsSeen.Contains(CurrentEndingID)) endingsSeen.Add(CurrentEndingID); return EndingIDToEndingData[CurrentEndingID].Summary; } }
  public Sprite Picture { get { if (!endingsSeen.Contains(CurrentEndingID)) endingsSeen.Add(CurrentEndingID); return EndingIDToEndingData[CurrentEndingID].Picture; } }
  public int TotalEndings { get => endings.Count; }
  public int EndingsSeen { get => endingsSeen.Count; }
  [field: SerializeField] public string CurrentEndingID { get; set; } = null;
  [field: SerializeField][field: HideInInspector] public float SecondsSpentInLevel { get; set; }
  [SerializeField] private List<string> endingsSeen = new();
  [SerializeField] private List<EndingData> endings = new();

  private Dictionary<string, EndingData> _endingIDToEndingData = null;
  private Dictionary<string, EndingData> EndingIDToEndingData {
    get {
      if (_endingIDToEndingData == null) {
        _endingIDToEndingData = new();
        foreach (var ending in endings) {
          _endingIDToEndingData[ending.EndingID] = ending;
        }
      }
      return _endingIDToEndingData;
    }
  }

  [System.Serializable]
  private class EndingData {
    [field: SerializeField] public string EndingID { get; set; }
    [field: SerializeField] public string Headline { get; set; }
    [field: SerializeField][field: TextArea] public string Summary { get; set; }
    [field: SerializeField] public Sprite Picture { get; set; }
  }
}
