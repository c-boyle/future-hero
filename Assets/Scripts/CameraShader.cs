using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraShader : MonoBehaviour {

  // [SerializeField] private Shader shader;
  // [field: SerializeField] public Camera Camera { get; set; }
  // private bool shaderSet = false;

  [SerializeField] private PostProcessVolume volume;
  [SerializeField] private Camera cam;
  private bool isEffectEnabled = false;
  private float currentVolumeWeight = 0;
  private float volumeTransitionSpeed = 0.01f;

  void Update() {
      if (isEffectEnabled && currentVolumeWeight < 0.99) {
          currentVolumeWeight = Mathf.Lerp(currentVolumeWeight, 1, volumeTransitionSpeed);
      } else if (!isEffectEnabled && currentVolumeWeight > 0.01) {
          currentVolumeWeight = Mathf.Lerp(currentVolumeWeight, 0, volumeTransitionSpeed);
      }

      volume.weight = currentVolumeWeight;
  }

  // public void ActivateShader(Shader shader = null) {
  //   if (shader != null) {
  //     Camera.SetReplacementShader(shader, string.Empty);
  //   } else {
  //     Camera.SetReplacementShader(this.shader, string.Empty);
  //   }
  // }

  // public void DeactivateShader() {
  //   Camera.ResetReplacementShader();
  //   shaderSet = false;
  // }

  // public void ToggleShader() {
  //   if (shaderSet) {
  //     DeactivateShader();
  //   } else {
  //     ActivateShader();
  //   }
  // }

  public void ToggleShader() {
      if (!isEffectEnabled) {
          isEffectEnabled = true;
          // onEffectActivate();
      }
      else {
          isEffectEnabled = false;
          // onEffectDeactivate();
      }
  }

}