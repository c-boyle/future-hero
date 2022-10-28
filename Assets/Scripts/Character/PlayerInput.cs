using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerInput : BaseInput {

  [SerializeField] private FutureSeer futureSeer;
  [SerializeField] private CameraBob cameraBob;
  [SerializeField] private CameraShader futureShader;
  [SerializeField] private AudioSource interactionAudio;
  [SerializeField] private FPSArmsManager FPSArmsManager;
  [SerializeField] private DialogueManager dialogueManager;

  public static ControlActions Controls;
  private bool activeMovementInput = false;
  private bool activeLookInput = false;
  private Interactable closestOutlinedInteractable;

  private void Awake() {
    if (Controls == null) {
      Controls = new();
    }

    // Controls that alter movement
    Controls.Player.Move.performed += ctx => activeMovementInput = true;
    Controls.Player.Move.canceled += ctx => { activeMovementInput = false; movement.Move(Vector2.zero); movement.ToggleSprint(false);};
    Controls.Player.Sprint.performed += ctx => movement.ToggleSprint(true);
    Controls.Player.Jump.performed += ctx => OnJump();
    Controls.Player.LookAtWatch.performed += ctx => { if ((!dialogueManager) || (!dialogueManager.isDialoging)) FPSArmsManager.isWatchShown = true; };
    Controls.Player.LookAtWatch.canceled += ctx => { if ((!dialogueManager) || (!dialogueManager.isDialoging)) FPSArmsManager.isWatchShown = false; };

    // Controls that alter vision
    Controls.Player.Look.performed += ctx => activeLookInput = true;
    Controls.Player.Look.canceled += ctx => { activeLookInput = false; movement.Look(Vector2.zero); };
    Controls.Player.ToggleVision.performed += ctx => OnToggleFutureVision();

    // Controls that affect environment
    Controls.Player.Interact.performed += ctx => OnInteract();
    Controls.Player.DropItem.performed += ctx => OnDropItem();

    Controls.Player.Pause.performed += ctx => {if ((!dialogueManager) || (!dialogueManager.isDialoging)) UIEventListener.Instance.OnPausePressed();};
  }

  private void Update() {
    Cursor.visible = false;
    if (dialogueManager && dialogueManager.isDialoging){
      return;
    }

    if (activeMovementInput) {
      movement.Move(Controls.Player.Move.ReadValue<Vector2>());
      cameraBob.isBobbing = true;
    } else {
      cameraBob.isBobbing = false;
    }

    if (activeLookInput) {
      movement.Look(Controls.Player.Look.ReadValue<Vector2>());
    }
    if (!futureSeer.TimeVisionEnabled) {
      var cameraTransform = Camera.main.transform;
      closestOutlinedInteractable = Interactable.GiveClosestInteractableInViewOutline(cameraTransform.position, cameraTransform.forward, itemHolder);
    }

  }

  private void OnEnable() {
    Controls.Enable();
    Controls.Player.Enable();
    Controls.UI.Disable();
  }

  private void OnDisable() {
    Controls.Disable();
  }

  private void OnDestroy() {
    OnDisable();
    Controls = null;
  }

  protected override void OnInteract() {
    // Interaction should be prohibited when looking into the future
    if (!futureSeer.TimeVisionEnabled) {
      var cameraTransform = Camera.main.transform;
      Interactable.UseClosestInteractableInView(cameraTransform.position, cameraTransform.forward, itemHolder);
      interactionAudio.Play();
    }
  }

  protected override void OnDropItem() {
    // Disable item dropping in the future?
    if (!futureSeer.TimeVisionEnabled) {
      base.OnDropItem();
    }
  }

  private void OnJump() {
    movement.Jump();
  }

  private void OnToggleFutureVision() {
    futureSeer.ToggleFutureVision();
    futureShader.ToggleShader();
    if (futureSeer.TimeVisionEnabled && closestOutlinedInteractable != null && closestOutlinedInteractable.shaderChanged) {
      closestOutlinedInteractable.toggleOutlineShader();
    }
  }
  public void SetFutureVision(bool enabled) {
    if (futureSeer.TimeVisionEnabled != enabled) {
      OnToggleFutureVision();
    }
  }

}
