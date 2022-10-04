using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerInput : BaseInput {

    private ControllerActions controls;
    private bool activeMovementInput = false;
    private bool activeLookInput = false;

    [SerializeField] private Interactable lever;

    private void Awake()
    {
        if (controls == null) {
            controls = new();
        }
        
        controls.Player.Move.performed += ctx => activeMovementInput = true;
        controls.Player.Move.canceled += ctx => { activeMovementInput = false; movement.Move(Vector2.zero); };

        controls.Player.Look.performed += ctx => activeLookInput = true;
        controls.Player.Look.canceled += ctx => { activeLookInput = false; movement.Look(Vector2.zero); };

        controls.Player.Interact.performed += ctx => OnInteract();
    }

    private void OnInteract()
    {

        if (Vector3.Distance(transform.position, lever.transform.position) <= 35f)
        {
            lever.Interact();
        }

    }

    private void Update()
    {
        Cursor.visible = false;

        if (activeMovementInput)
        {
            movement.Move(controls.Player.Move.ReadValue<Vector2>());
        }

        if (activeLookInput)
        {
            movement.Look(controls.Player.Look.ReadValue<Vector2>());
        }


        // float mouseX = Input.GetAxis("Mouse X");
        // float mouseY = Input.GetAxis("Mouse Y");

        // Vector2 rotation = new Vector2(mouseX, mouseY);
        // movement.Look(rotation);
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
