using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerInput : BaseInput {

  [SerializeField] private FutureSeer futureSeer;
  [SerializeField] private CameraBob cameraBob;
  [SerializeField] private CameraShader futureShader;

  private ControlActions controls;
  private bool activeMovementInput = false;
  private bool activeLookInput = false;

  private void Awake() {
    if (controls == null) {
      controls = new();
    }

    // Controls that alter movement
    controls.Player.Move.performed += ctx => activeMovementInput = true;
    controls.Player.Move.canceled += ctx => { activeMovementInput = false; movement.Move(Vector2.zero); };
    controls.Player.Jump.performed += ctx => OnJump();
    controls.Player.LookAtWatch.performed += ctx => movement.LookAtWatch();
    controls.Player.LookAtWatch.canceled += ctx => movement.PutWatchAway();  

    // Controls that alter vision
    controls.Player.Look.performed += ctx => activeLookInput = true;
    controls.Player.Look.canceled += ctx => { activeLookInput = false; movement.Look(Vector2.zero); };
    controls.Player.ToggleVision.performed += ctx => OnToggleFutureVision();

    // Controls that affect environment
    controls.Player.Interact.performed += ctx => OnInteract();
    controls.Player.DropItem.performed += ctx => OnDropItem();
  }

  private void Update() {
    Cursor.visible = false;
    if (activeMovementInput) {
      movement.Move(controls.Player.Move.ReadValue<Vector2>());
      cameraBob.isBobbing = true;
    } else {
      cameraBob.isBobbing = false;
    }

    if (activeLookInput){
        movement.Look(controls.Player.Look.ReadValue<Vector2>());
    }
    var cameraTransform = Camera.main.transform;
    Interactable.GiveClosestInteractableInViewOutline(cameraTransform.position, cameraTransform.forward, itemHolder);
  }

  private void OnEnable() {
    controls.Enable();
  }

  private void OnDisable() {
    controls.Disable();
  }

  protected override void OnInteract() {
    // Interaction should be prohibited when looking into the future
    if (!futureSeer.TimeVisionEnabled) {
      var cameraTransform = Camera.main.transform;
      Interactable.UseClosestInteractableInView(cameraTransform.position, cameraTransform.forward, itemHolder);
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
  }
  public void SetFutureVision(bool enabled) {
    if (futureSeer.TimeVisionEnabled != enabled) {
      OnToggleFutureVision();
    }
  }

}
