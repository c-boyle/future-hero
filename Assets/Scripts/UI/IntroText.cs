using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroText : MonoBehaviour
{
    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private DialogueManager dialogueManager;
    private bool started = false;

    void Start()
    {
        UIEventListener.Instance.PauseGame();
        PlayerInput.Controls.Player.Disable();
        dialogueTrigger.TriggerDialogue();
    }

    void Update()
    {
        Proceed();
    }

    void Proceed() {
        if (!dialogueManager || !dialogueManager.isDialoging) return; // Skip if we're done intro-ing

        if (dialogueManager.finalDialogue) {
            PlayerInput.Controls.Player.Enable(); // Enable controls to go in future
            return; // To stop anything else from triggering the next sentence
        }

        if (Input.anyKeyDown){
            dialogueManager.NextSentence();
        }
    }

    public void StartGame() {
        if (started) return;

        dialogueManager.EndDialogue();
        PlayerInput.Controls.Player.Enable();
        UIEventListener.Instance.UnpauseGame(); 
        started = true;
    }
}
