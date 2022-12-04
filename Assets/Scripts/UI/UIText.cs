using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIText : MonoBehaviour
{
    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private DialogueManager dialogueManager;
    private bool started = false;

    public void TriggerSpecificDialogue(DialogueTrigger trigger)
    {
        dialogueTrigger = trigger;
        dialogueTrigger.TriggerDialogue();
    }

    void Start() {
        UIEventListener.Instance.PauseGame();
        EnableControls(false);
    }

    void Update()
    {
        Proceed();
    }

    void Proceed() {
        if (!dialogueManager || !dialogueManager.isDialoging) return; // Skip if we're done intro-ing

        if (dialogueManager.finalDialogue) {
            PlayerInput.Controls.Player.Enable(); // Enable controls so we can go in future
            return; // To stop anything else from triggering the next sentence
        }

        if (Input.anyKeyDown){
            dialogueManager.NextSentence();
        }
    }

    public void StartGame() {
        if (started) return;

        dialogueManager.EndDialogue();
        EnableControls(true);
        UIEventListener.Instance.UnpauseGame(); 
        started = true;
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
