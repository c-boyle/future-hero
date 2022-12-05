using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIText : MonoBehaviour
{
    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private DialogueManager dialogueManager;
    private InputAction freeingAction = null; 
    private bool gaming = false;
    public Action<InputAction.CallbackContext> handler;

    void Awake() {
        handler = (InputAction.CallbackContext ctx) => Unpause(ctx);
    }

    void Start() {
        FreeingAction("move");
        TriggerSpecificDialogue(dialogueTrigger);
    }

    public void TriggerSpecificDialogue(DialogueTrigger trigger)
    {
        EnableControls(false);
        dialogueManager.EndDialogue();
        dialogueTrigger = trigger;
        gaming = false;
        dialogueTrigger.TriggerDialogue();
        UIEventListener.Instance.PauseGame();
    }

    private void SetFreeingAction(InputAction action = null) {
        if(freeingAction != null) freeingAction.performed -= handler;
        freeingAction = action;
        if(freeingAction != null) freeingAction.performed += handler;
    }

    public void FreeingAction(string action) {
        switch(action) {
        case "move":
            SetFreeingAction(PlayerInput.Controls.Player.Move);
            break;
        case "look":
            SetFreeingAction(PlayerInput.Controls.Player.Look);
            break;
        case "future":
            SetFreeingAction(PlayerInput.Controls.Player.ToggleVision);
            break;
        case "jump":
            SetFreeingAction(PlayerInput.Controls.Player.Jump);
            break;
        default:
            SetFreeingAction();
            break;
        }

    }

    void Update()
    {
        Proceed();
    }

    void Proceed() {
        if (!dialogueManager || !dialogueManager.isDialoging) return; // Skip if we're not 'dialoging'

        if (dialogueManager.finalDialogue && freeingAction != null) {
            PlayerInput.Controls.Player.Enable(); // Enable controls so we can use freeing action
            return; // To stop anything else from triggering the next sentence
        }

        if (Input.anyKeyDown){
            dialogueManager.NextSentence();
        }
    }

    public void Unpause(InputAction.CallbackContext ctx) {
        if (gaming) return;

        dialogueManager.EndDialogue();
        EnableControls(true);
        UIEventListener.Instance.UnpauseGame(); 
        gaming = true;
    }

    public void EnableControls(bool enable) {
        if (enable) {
            PlayerInput.Controls.Player.Enable();
            PlayerInput.Controls.UI.Enable();
        } else {
            PlayerInput.Controls.Player.Disable();
            PlayerInput.Controls.UI.Disable();
        }
    }
}