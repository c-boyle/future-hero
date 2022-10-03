using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShader : MonoBehaviour {

  [SerializeField] private Shader shader;
  [SerializeField] private Camera cam;
  private bool shaderSet = false;

  public void ActivateShader(Shader shader = null) {
    if (shader != null) {
      cam.SetReplacementShader(shader, string.Empty);
    } else {
      cam.SetReplacementShader(this.shader, string.Empty);
    }
    shaderSet = true;
  }

  public void DeactivateShader() {
    cam.ResetReplacementShader();
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
