using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerInput : BaseInput {

  [SerializeField] private FutureSeer futureSeer;
  [SerializeField] private CameraShader futureShader;

  private ControlActions controls;
  private bool activeMovementInput = false;
  private bool activeLookInput = false;

  private float distanceToOutline = 15f;

  private void Awake() {
    if (controls == null) {
      controls = new();
    }

    // Controls that alter movement
    controls.Player.Move.performed += ctx => activeMovementInput = true;
    controls.Player.Move.canceled += ctx => { activeMovementInput = false; movement.Move(Vector2.zero); };
    controls.Player.Jump.performed += ctx => OnJump();

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
    }

    if (activeLookInput){
        movement.Look(controls.Player.Look.ReadValue<Vector2>());
    }
    var cameraTransform = futureShader.Camera.transform;
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
      var cameraTransform = futureShader.Camera.transform;
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
