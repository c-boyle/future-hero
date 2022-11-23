using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : BaseInput {
  [SerializeField] private ViewBob viewBob;
  [SerializeField] private FutureSeer futureSeer;
  [SerializeField] private AudioSource interactionAudio;
  [SerializeField] private FPSArmsManager FPSArmsManager;
  [SerializeField] private DialogueManager dialogueManager;
  [SerializeField] private LineRenderer trajectoryRenderer;

  [SerializeField] private Watch watch;

  public static ControlActions Controls;
  private bool activeMovementInput = false;
  private bool activeLookInput = false;
  private bool isSprinting = false;
  private Interactable closestOutlinedInteractable;
  private float pickupTime = 0;
  private bool holdingDrop = false;

  // Constants
  private const float INITIAL_SPEED_MULTIPLIER = 1f;
  private const float SPRINT_SPEED_MULTIPLIER = 1.3f;

  private void Awake() {
    if (Controls == null) {
      Controls = new();
    }
    Cursor.visible = false;

    // Controls that alter movement
    Controls.Player.Move.performed += ctx => activeMovementInput = true;
    Controls.Player.Move.canceled += ctx => { activeMovementInput = false; movement.Move(Vector2.zero); movement.sprintMultiplier = INITIAL_SPEED_MULTIPLIER; };
    Controls.Player.Sprint.performed += ctx => isSprinting = true;
    Controls.Player.Sprint.canceled += ctx => isSprinting = false;
    Controls.Player.Jump.performed += ctx => { OnJump(); };
    Controls.Player.LookAtWatch.performed += ctx => { if ((!dialogueManager) || (!dialogueManager.isDialoging)) FPSArmsManager.isWatchShown = true; watch.LookingAt(true);};
    Controls.Player.LookAtWatch.canceled += ctx => { if ((!dialogueManager) || (!dialogueManager.isDialoging)) FPSArmsManager.isWatchShown = false; watch.LookingAt(false);};

    // Controls that alter vision
    Controls.Player.Look.performed += ctx => activeLookInput = true;
    Controls.Player.Look.canceled += ctx => { activeLookInput = false; movement.Look(Vector2.zero); };
    Controls.Player.ToggleVision.performed += ctx => OnToggleFutureVision();

    // Controls that affect environment
    Controls.Player.Interact.performed += ctx => OnInteract();
    Controls.Player.PickDrop.performed += ctx => { pickupTime = Time.time; holdingDrop = true; };
    Controls.Player.PickDrop.canceled += ctx => { OnPickDropItem(Time.time - pickupTime); holdingDrop = false; itemHolder.ClearThrowTrajectory(trajectoryRenderer); };
    

    Controls.Player.Pause.performed += ctx => {if ((!dialogueManager) || (!dialogueManager.isDialoging)) UIEventListener.Instance.OnPausePressed();};
  }

  private void Update() {
    if (dialogueManager && dialogueManager.isDialoging){
      return;
    }
    if (activeMovementInput) {
      movement.Move(Controls.Player.Move.ReadValue<Vector2>());
    }
    if (isSprinting) {
      movement.isSprintEnabled = true;
      movement.sprintMultiplier = SPRINT_SPEED_MULTIPLIER;
      viewBob.globalSpeedMultiplier = SPRINT_SPEED_MULTIPLIER;
    } else {
      movement.isSprintEnabled = false;
      movement.sprintMultiplier = INITIAL_SPEED_MULTIPLIER;
      viewBob.globalSpeedMultiplier = INITIAL_SPEED_MULTIPLIER;
    }
    if (activeLookInput) {
      movement.Look(Controls.Player.Look.ReadValue<Vector2>());
    }
    if (holdingDrop && Time.time - pickupTime >= 0.1f) {
      itemHolder.DrawThrowTrajectory(trajectoryRenderer, Time.time - pickupTime);
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

  protected override void OnPickDropItem(float windup = 0) {
    // Disable item dropping in the future?
    if (!futureSeer.TimeVisionEnabled) {
      base.OnPickDropItem(windup);
    }
  }

  private void OnJump() {
    movement.Jump();
  }

  private void OnToggleFutureVision() {
    futureSeer.ToggleFutureVision();
    if (futureSeer.TimeVisionEnabled && closestOutlinedInteractable != null && closestOutlinedInteractable.shaderChanged) {
      closestOutlinedInteractable.toggleOutlineShader(itemHolder);
    }
  }
  public void SetFutureVision(bool enabled) {
    if (futureSeer.TimeVisionEnabled != enabled) {
      OnToggleFutureVision();
    }
  }

}
