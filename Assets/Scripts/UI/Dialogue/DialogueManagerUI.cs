using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManagerUI : DialogueManager
{
    [SerializeField] private TextMeshProUGUI field;

    protected override IEnumerator DisplaySentence(string sentence) {
        field.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            field.text += letter;
            yield return null;
        }
    }

    protected override void ClearSentence() {
        field.text = "";
        return;
    }
}
