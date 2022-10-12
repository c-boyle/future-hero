using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShader : MonoBehaviour {

  [SerializeField] private Shader shader;
  [field: SerializeField] public Camera Camera { get; set; }
  private bool shaderSet = false;

  public void ActivateShader(Shader shader = null) {
    if (shader != null) {
      Camera.SetReplacementShader(shader, string.Empty);
    } else {
      Camera.SetReplacementShader(this.shader, string.Empty);
    }
    shaderSet = true;
  }

  public void DeactivateShader() {
    Camera.ResetReplacementShader();
    shaderSet = false;
  }

  public void ToggleShader() {
    if (shaderSet) {
      DeactivateShader();
    } else {
      ActivateShader();
    }
  }
}
