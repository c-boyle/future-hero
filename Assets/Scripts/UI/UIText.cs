using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class UIText : MonoBehaviour
{
    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private InputAction freeingAction = null; 
    public string freeingStringAction = "";
    [SerializeField] private bool actionPresent = false;

    [SerializeField] private TMP_Text nextPrompt;

    private bool gaming = false;
    public Action<InputAction.CallbackContext> handler;
    [SerializeField] private GameObject screenDarkener;
    private string finalText = "";

    void Awake() {
        handler = (InputAction.CallbackContext ctx) => Unpause(ctx);
    }

    void Start() {
        if(SettingsInitializer.Instance.IsTutorial){
            FreeingAction("move");
            TriggerSpecificDialogue(dialogueTrigger);
        }
    }

    public void TriggerSpecificDialogue(DialogueTrigger trigger)
    {
        screenDarkener.SetActive(true);
        nextPrompt.gameObject.SetActive(true);
        
        EnableControls(false);
        dialogueManager.EndDialogue();
        dialogueTrigger = trigger;
        gaming = false;
        dialogueTrigger.TriggerDialogue();
        UIEventListener.Instance.PauseGame();
    }

    private void SetFreeingAction(InputAction action = null) {
        actionPresent = true;
        if(freeingAction != null) freeingAction.performed -= handler;
        freeingAction = action;
        if(freeingAction != null) freeingAction.performed += handler;
    }

    public void FreeingAction(string action) {
        finalText = string.Empty;
        freeingStringAction = action;
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
        case "interact":
            SetFreeingAction(PlayerInput.Controls.Player.Interact);
            break;
        case "drop":
        case "pickup":
            SetFreeingAction(PlayerInput.Controls.Player.PickDrop);
            break;
        case "lookat":
            SetFreeingAction(PlayerInput.Controls.Player.LookAtWatch);
            break;
        default:
            finalText = "(press anything to continue)";
            actionPresent = false;
            break;
        }

    }

    void Update()
    {
        Proceed();
    }

    void Proceed() {
        if (!dialogueManager || !dialogueManager.isDialoging) return; // Skip if we're not 'dialoging'

        if(dialogueManager.finalDialogue) nextPrompt.text = finalText;

        if (dialogueManager.finalDialogue && actionPresent) {
            if(Input.anyKeyDown) dialogueManager.FinishSentence();
            PlayerInput.Controls.Player.Enable(); // Enable controls so we can use freeing action
            return; // To stop anything else from triggering the next sentence
        }

        if (Input.anyKeyDown){
            if (dialogueManager.finalDialogue) Unpause();
            dialogueManager.NextSentence();
        }
    }

    public void Unpause(InputAction.CallbackContext ctx) {
        Unpause();
    }

    public void Unpause() {
        if (gaming) return;

        screenDarkener.SetActive(false);
        nextPrompt.gameObject.SetActive(false);

        nextPrompt.text = "(press anything to continue)";
        finalText = string.Empty;
        actionPresent = false;

        

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