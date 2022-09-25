using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerInput : BaseInput {

    private PlayerControls controls;
    private bool activeMovementInput = false;

    [SerializeField] private Interactable lever;

    private void Awake()
    {
        if (controls == null) {
            controls = new();
        }
        
        controls.Player.Move.performed += ctx => activeMovementInput = true;
        controls.Player.Move.canceled += ctx => { activeMovementInput = false; movement.Move(Vector2.zero); };
        controls.Player.Interact.performed += ctx => OnInteract();
    }

    private void OnInteract()
    {

        if (Vector3.Distance(transform.position, lever.transform.position) <= 10f)
        {
            lever.Interact();
        }

    }

    private void Update()
    {
        if (activeMovementInput)
        {
            movement.Move(controls.Player.Move.ReadValue<Vector2>());
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
