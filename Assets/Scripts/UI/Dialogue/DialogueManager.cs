using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    [SerializeField] private UnityEvent onDialogueOver;

    private Queue<string> sentences = new Queue<string>();
    public bool isDialoging = false; // true when we are currently showing dialogue
    public bool finalDialogue = false; // true when the final piece of dialogue IS output

    public void StartDialogue(Dialogue dialogue)
    {
        isDialoging = true;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        NextSentence();
    }

    public void NextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
        }
        
        if (!isDialoging) {
            return;
        }
        
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(DisplaySentence(sentence));

        if (sentences.Count == 0) {
            finalDialogue = true;
        }
    }

    protected virtual IEnumerator DisplaySentence(string sentence) {
        Debug.Log("sentence: " + sentence);
        return null;
    }

    protected virtual void ClearSentence() {
        Debug.Log("Clear!");
        return;
    }

    public void EndDialogue() {
        StopAllCoroutines();
        isDialoging = false;
        finalDialogue = false;
        ClearSentence();
        sentences.Clear();
        onDialogueOver?.Invoke();
    }

}
