using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerInput : BaseInput {

  private PlayerControls controls;
  private bool activeMovementInput = false;

  public static event EventHandler<Interactable.InteractionEventArgs> PlayerInteraction;

  private void Awake() {
    if (controls == null) {
      controls = new();
    }

    controls.Player.Move.performed += ctx => activeMovementInput = true;
    controls.Player.Move.canceled += ctx => { activeMovementInput = false; movement.Move(Vector2.zero); };
    controls.Player.Interact.performed += ctx => OnInteract();
  }

  private void OnInteract() {
    PlayerInteraction?.Invoke(this, new Interactable.InteractionEventArgs() { InteractorPosition = transform.position });
  }

  private void Update() {
    if (activeMovementInput) {
      movement.Move(controls.Player.Move.ReadValue<Vector2>());
    }

    Cursor.visible = false;

    float mouseX = Input.GetAxis("Mouse X");
    float mouseY = Input.GetAxis("Mouse Y");

    Vector2 rotation = new Vector2(mouseX, mouseY);
    movement.Look(rotation);
  }

  private void OnEnable() {
    controls.Enable();
  }

  private void OnDisable() {
    controls.Disable();
  }
}
