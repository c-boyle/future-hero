using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPrompt : MonoBehaviour {
  public enum ControlType {
    Gamepad,
    Keyboard
  }

  [SerializeField] private GameObject gamepadButtonVisual;
  [SerializeField] private GameObject keyboardKeyVisual;

  private static readonly HashSet<ControlsPrompt> controlsPrompts = new();

  private static ControlType _controlType = ControlType.Gamepad;
  public static ControlType DisplayedControlType {
    get => _controlType;
    set {
      _controlType = value;
      foreach (ControlsPrompt prompt in controlsPrompts) {
        prompt.SetControlType(_controlType);
      }
    }
  }

  public static void ChangeControlType(ControlType type) {
    DisplayedControlType = type;
  }

  private void Start() {
    controlsPrompts.Add(this);
    SetControlType(_controlType);
  }

  private void OnDestroy() {
    controlsPrompts.Remove(this);
  }

  private void SetControlType(ControlType controlType) {
    gamepadButtonVisual.SetActive(controlType == ControlType.Gamepad);
    keyboardKeyVisual.SetActive(controlType == ControlType.Keyboard);
  }
}
