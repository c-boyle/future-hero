using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManagerUI : DialogueManager
{
    [SerializeField] private TMP_Text field;
    public bool giveBackground = true;

    private const string TEXT_BACKGROUND_HEX = "#00000088";

    protected override IEnumerator DisplaySentence(string sentence) {
        field.text = "";
        string displayedText = "";
        foreach (char letter in sentence.ToCharArray())
        {
            displayedText += letter;
            SetText(displayedText);
            yield return null;
        }
        currentSentence = "";
    }

    public override void FinishSentence() {
        if(currentSentence == "") return;
        StopAllCoroutines();
        SetText(currentSentence);
        currentSentence = "";
        return;
    }

    protected override void ClearSentence() {
        field.text = "";
        return;
    }

    private void SetText(string txt) {
        field.text = (giveBackground) ? $"<mark={TEXT_BACKGROUND_HEX}>" + txt + "</mark>" : txt;
    }
}
